'******************************************************************************************************************
' carce: SyncroSim Base Package for simulating the cumulative effects of development and climate change on caribou.
'
' Copyright © 2007-2019 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'******************************************************************************************************************

Imports SyncroSim.Core
Imports System.Globalization

Class Bridge2
    Inherits Transformer

    Const ITERATION_COLUMN_NAME As String = "Iteration"
    Const TIMESTEP_COLUMN_NAME As String = "Timestep"
    Const AGE_CLASS_ID_COLUMN_NAME As String = "AgeClassID"
    Const FECUNDITY_COLUMN_NAME As String = "Fecundity"
    Const MORTALITY_COLUMN_NAME As String = "Mortality"

    Private Shared Sub ThrowConfigException(ByVal msg As String)
        ThrowArgumentException("Energy Protein to Population Configuration: {0}", msg)
    End Sub

    Public Overrides Sub Transform()

        Dim ds As DataSheet = Me.ResultScenario.GetDataSheet("CCE_EPToPop")
        Dim dr As DataRow = ds.GetDataRow()

        If (dr Is Nothing OrElse
            dr("BaselineCalfWeight") Is DBNull.Value OrElse
            dr("BaselineCowWeight") Is DBNull.Value OrElse
            dr("RefCowBodyWeight") Is DBNull.Value OrElse
            dr("BodyWeightProb1") Is DBNull.Value OrElse
            dr("BodyWeightProb2") Is DBNull.Value OrElse
            dr("JulianDayConception") Is DBNull.Value OrElse
            dr("CalfAdjustmentFactor") Is DBNull.Value OrElse
            dr("CalfAgeClass") Is DBNull.Value) Then

            Me.ResultScenario.RecordStatus(StatusType.Information,
                "Not running Transformer because the configuration is incomplete.")

            Return

        End If

        Dim BaselineCalfWeight As Integer = CInt(dr("BaselineCalfWeight"))
        Dim BaselineCowWeight As Integer = CInt(dr("BaselineCowWeight"))
        Dim RefCowBodyWeight As Double = CDbl(dr("RefCowBodyWeight"))
        Dim BodyWeightProb1 As Double = CDbl(dr("BodyWeightProb1"))
        Dim BodyWeightProb2 As Double = CDbl(dr("BodyWeightProb2"))
        Dim JulianDayConception As Integer = CInt(dr("JulianDayConception"))
        Dim CalfAdjustmentFactor As Double = CDbl(dr("CalfAdjustmentFactor"))
        Dim CalfAgeClass As Integer = CInt(dr("CalfAgeClass"))

        'Base Line
        Dim BaseLineRatio As Double = BaselineCowWeight / RefCowBodyWeight
        Dim BaseLineProbPreg As Double = CalcProbPreg(BaseLineRatio, BodyWeightProb1, BodyWeightProb2)

        'Target
        Dim TargetAverageWTBODY As Double = Me.CalcAverageWT(JulianDayConception, Me.ResultScenario.Id, "WTBODY")
        Dim TargetRatio As Double = TargetAverageWTBODY / RefCowBodyWeight
        Dim TargetProbPreg As Double = CalcProbPreg(TargetRatio, BodyWeightProb1, BodyWeightProb2)

        'Average Weights
        Dim TargetAverageWTCALF As Double = Me.CalcAverageWT(JulianDayConception, Me.ResultScenario.Id, "WTCALF")

        'Adjustment Factors
        Dim FecundityAdjustmentFactor As Double = TargetProbPreg - BaseLineProbPreg
        Dim MortalityAdjustmentFactor As Double = (BaselineCalfWeight - TargetAverageWTCALF) * CalfAdjustmentFactor

        'Update DG-Sim's demographic rate shift
        Me.UpdateDemographicRateShift(FecundityAdjustmentFactor, MortalityAdjustmentFactor, CalfAgeClass)

    End Sub

    Private Function CalcAverageWT(
        ByVal julianDateOfConception As Integer,
        ByVal scenarioId As Integer,
        ByVal columnName As String) As Double

        Using store As DataStore = Me.Library.CreateDataStore

            Dim q As String = String.Format(CultureInfo.InvariantCulture,
                "SELECT {0} FROM CAREP_OutTimestep WHERE ScenarioID={1} AND Timestep={2} AND ({3} > 0.0) AND (LACT = 1.0)",
                columnName, scenarioId, julianDateOfConception, columnName)

            Dim dt As DataTable = store.CreateDataTableFromQuery(q, "AverageWT")

            If (dt.Rows.Count = 0) Then

                ThrowArgumentException(
                    "There is no output for the scenario on the julian date of conception: Scenario ID {0} -> {1}",
                    scenarioId, columnName)

            End If

            Dim total As Double = 0.0

            For Each dr As DataRow In dt.Rows
                total += CDbl(dr(columnName))
            Next

            Return (total / dt.Rows.Count)

        End Using

    End Function

    Private Shared Function CalcProbPreg(
        ByVal ratio As Double,
        ByVal bodyWeightProb1 As Double,
        ByVal bodyWeightProb2 As Double) As Double

        Dim v1 As Double = bodyWeightProb1 + bodyWeightProb2 * ratio
        Dim v2 As Double = bodyWeightProb1 + bodyWeightProb2 * ratio

        Dim d As Double = Math.Exp(v1) / (1 + (Math.Exp(v2)))
        Debug.Assert(d >= 0.0 And d <= 1.0)

        Return d

    End Function

    Private Sub UpdateDemographicRateShift(
        ByVal fecundityAdjustmentFactor As Double,
        ByVal mortalityAdjustmentFactor As Double,
        ByVal calfAgeClassId As Integer)

        Debug.Assert(fecundityAdjustmentFactor >= -1.0 And fecundityAdjustmentFactor <= 1.0)
        Debug.Assert(mortalityAdjustmentFactor >= -1.0 And mortalityAdjustmentFactor <= 1.0)

        Dim ds As DataSheet = Me.ResultScenario.GetDataSheet("DGSim_DemographicRateShift")
        Dim dt As DataTable = ds.GetData()

        AddOrUpdateFecundityRecord(dt, fecundityAdjustmentFactor)
        AddOrUpdateMortalityRecord(dt, mortalityAdjustmentFactor, calfAgeClassId)

#If DEBUG Then
        VALIDATE_DRS_TABLE(dt)
#End If

    End Sub

    Private Shared Sub AddOrUpdateFecundityRecord(
        ByVal dt As DataTable,
        ByVal fecundityAdjustmentFactor As Double)

        Dim dr As DataRow = FindNullAgeClassRow(dt)

        If (dr Is Nothing) Then

            dr = dt.NewRow
            dt.Rows.Add(dr)

        End If

        dr(ITERATION_COLUMN_NAME) = DBNull.Value
        dr(TIMESTEP_COLUMN_NAME) = DBNull.Value
        dr(AGE_CLASS_ID_COLUMN_NAME) = DBNull.Value
        dr(FECUNDITY_COLUMN_NAME) = fecundityAdjustmentFactor
        dr(MORTALITY_COLUMN_NAME) = DBNull.Value

    End Sub

    Private Shared Sub AddOrUpdateMortalityRecord(
        ByVal dt As DataTable,
        ByVal mortalityAdjustmentFactor As Double,
        ByVal calfAgeClassId As Integer)

        Dim dr As DataRow = FindAgeClassRow(dt, calfAgeClassId)

        If (dr Is Nothing) Then

            dr = dt.NewRow
            dt.Rows.Add(dr)

        End If

        dr(ITERATION_COLUMN_NAME) = DBNull.Value
        dr(TIMESTEP_COLUMN_NAME) = DBNull.Value
        dr(AGE_CLASS_ID_COLUMN_NAME) = calfAgeClassId
        dr(FECUNDITY_COLUMN_NAME) = DBNull.Value
        dr(MORTALITY_COLUMN_NAME) = mortalityAdjustmentFactor

    End Sub

    Private Shared Function FindNullAgeClassRow(ByVal dt As DataTable) As DataRow

        For Each dr As DataRow In dt.Rows

            If (dr(AGE_CLASS_ID_COLUMN_NAME) Is DBNull.Value) Then
                Return dr
            End If

        Next

        Return Nothing

    End Function

    Private Shared Function FindAgeClassRow(ByVal dt As DataTable, ByVal ageClassId As Integer) As DataRow

        For Each dr As DataRow In dt.Rows

            If (dr(AGE_CLASS_ID_COLUMN_NAME) IsNot DBNull.Value) Then

                Dim id As Integer = CInt(dr(AGE_CLASS_ID_COLUMN_NAME))

                If (id = ageClassId) Then
                    Return dr
                End If

            End If

        Next

        Return Nothing

    End Function

    Private Shared Sub ThrowArgumentException(message As [String])
        ThrowArgumentException(message, New Object() {})
    End Sub

    Private Shared Sub ThrowArgumentException(message As [String], ParamArray args As Object())
        Throw New ArgumentException(String.Format(CultureInfo.InvariantCulture, message, args))
    End Sub

#If DEBUG Then

    Private Shared Sub VALIDATE_DRS_TABLE(ByVal dt As DataTable)

        Dim c As Integer = 0
        Dim d As New Dictionary(Of Integer, Boolean)

        For Each dr As DataRow In dt.Rows

            If (dr(AGE_CLASS_ID_COLUMN_NAME) Is DBNull.Value) Then
                c += 1
            Else
                d.Add(CInt(dr(AGE_CLASS_ID_COLUMN_NAME)), True)
            End If

        Next

        Debug.Assert(c = 0 Or c = 1)

    End Sub

#End If

End Class
