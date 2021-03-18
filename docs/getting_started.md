---
layout: default
title: Getting started
---

# Getting started with **Carce**

## How to Install

These instructions are for the [most recent full release of carce (1.0.12)](https://github.com/ApexRMS/carce/releases)

1. Download and install [SyncroSim](https://www.syncrosim.com/downld) for Windows version 2.0.42
2. Download the following [SyncroSim Packages](https://www.syncrosim.com/packages): carep(2.1.40), carmove (1.0.12), dgsim (2.1.39), carce (1.0.12)
3. Open SyncroSim: **Start > Programs > SyncroSim**
4. Install the following SyncroSim Packages (in this order): carep, carmove, dgsim, carce. **Note** to install a package in SyncroSim go to **File > Packages > Add...** and point to the *ssimpkg* file you downloaded in step 2.
5. To open a carce SyncroSim library go to: **File > Open Library...** and point to the appropriate *.ssim file.
6. The final two steps are only required if you plan to run the caribou movement model (carmove) within carce. The carce package currently requires that you download and install [QGIS 2.4.0-1](http://download.osgeo.org/qgis/windows/) for Windows x64.
7. To ensure that the carmove package points to the correct QGIS installation navigate to *...Users\[username]\Documents\SyncroSim\Modules\carmove\Scripts* and edit the file called ***caribouMovement.bat***. The first line should read:

   ```
   SET OSGEO4W_ROOT=C:\Program Files\QGIS Chugiak
   ```
