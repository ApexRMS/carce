'******************************************************************************************************
' CCE: A SyncroSim Module for simulating cumulative impacts of multiple factors on caribou populations.
'
' Copyright © 2007-2018 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'******************************************************************************************************

Imports SyncroSim.Core
Imports System.Globalization

Class Bridge1
    Inherits Transformer

    Public Overrides Sub Transform()

        Dim ds As DataSheet = Me.ResultScenario.GetDataSheet("CAREP_Location")

        Dim query As String = String.Format(CultureInfo.InvariantCulture,
            "INSERT INTO CAREP_Location(ScenarioID, Iteration, Timestep, JulianDay, StratumID) " &
            "SELECT {0}, Iteration, Timestep, JulianDay, CCE_StratumCrosswalk.CAREPStratumID FROM CM_OutputLocation " &
            "INNER JOIN CCE_StratumCrosswalk ON CM_OutputLocation.StratumID=CCE_StratumCrosswalk.CMStratumID " &
            "WHERE ScenarioID={1}", Me.ResultScenario.Id, Me.ResultScenario.Id)

        Using scope As SyncroSimTransactionScope = Session.CreateTransactionScope

            Using store As DataStore = Me.Library.CreateDataStore

                'Clear the CAREP_Location data

                ds.ClearData()
                ds.Changes.Add(New ChangeRecord(Me, "CCE Deleted Rows"))

                Me.Library.Save(store)

                'Transfer the CM_OutputLocation data to CAREP_Location using the stratum crosswalk

                store.ExecuteNonQuery(query)

                'Load the new CAREP_Location data

                ds.ResetData()
                ds.GetData(store)

            End Using

            scope.Complete()

        End Using

    End Sub

End Class
