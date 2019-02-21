# carce

SyncroSim Base Package for simulating the cumulative effects of development and climate change on caribou.

Learn more at http://www.apexrms.com/caribou-simulation-model/.

[![carce Video](https://img.youtube.com/vi/eYjAEqdovJM/0.jpg)](https://www.youtube.com/watch?v=eYjAEqdovJM "Overview of carce")



## Installation

These instructions are for the [most recent full release of carce (1.0.12)](https://github.com/ApexRMS/carce/releases)

1. Install [SyncroSim](www.syncrosim.com/download) for Windows version 2.0.42

2. Download the following [SyncroSim Packages](www.syncrosim.com/packages): carep (2.1.40), carmove (1.0.12), dgsim (2.1.39), carce (1.0.12)

3. 

4. Open SyncroSim: **Start | Programs | SyncroSim**

5. Install the following SyncroSim Packages (in this order): carep, carmove, dgsim, carce. **Note **to install a package in SyncroSim go to **File | Packages | Add...** and point to the *ssimpkg* file you downloaded in step 2.

6. To open a carce SyncroSim library go to: **File | Open Library...** and point to the appropriate *.ssim file.  

7. The final two steps are only required if you plan to run the caribou movement model (carmove) within carce.  The carce package currently requires that you download and install [QGIS 2.4.0-1](http://download.osgeo.org/qgis/windows/) for Windows x64

8. To ensure that the carmove package points to the correct QGIS installation navigate to *...Users\\[username]\Documents\SyncroSim\Modules\carmove\Scripts* and edit the file called ***caribouMovement.bat***. The first line should read:

   ```
   SET OSGEO4W_ROOT=C:\Program Files\QGIS Chugiak
   ```

   If this is not the root installation for QGIS 2.4.0 on your system, change this line to point to the root installation. Also confirm that inside the root installation there is a file inside the ***bin*** directory called ***o4w_env.bat***. If this file is not located here, find its location and change the second line in the batch file from:

   ```
   CALL "%OSGEO4W_ROOT%\bin\o4w_env.bat"
   ```

   to point to the correct location of the ***o4w_env.bat*** file.


