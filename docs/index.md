---
layout: default
title: Home
description: "Landing page for the Package"
permalink: /
---

# **Carce** SyncroSim Package
<img align="right" style="padding: 13px" width="180" src="assets/images/logo/carce-sticker.png">
[![GitHub release](https://img.shields.io/github/v/release/ApexRMS/carce.svg?style=for-the-badge&color=d68a06)](https://GitHub.com/ApexRMS/carce/releases/)    <a href="https://github.com/ApexRMS/carce"><img align="middle" style="padding: 1px" width="30" src="assets/images/logo/github-trans2.png">
<br>
## Evaluating the effects of multiple stressors on caribou
### *Carce* is an open-source [SyncroSim](https://syncrosim.com/){:target="_blank"} Base Package for simulating the cumulative effects of development and climate change on caribou.

The **Carce** package contains a built-in stochastic multi-model framework for forecasting the cumulative effects of stressors on barren-ground caribou herds, referred to as the **Caribou Cumulative Effects (CCE)** modeling framework. Built upon the [SyncroSim](https://syncrosim.com/){:target="_blank"} software platform, the **CCE** modeling framework was developed with the intent of providing users with the tools required to make better-informed management decisions for caribou herds in the face of future uncertainties. As such, the **CCE** modeling framework contains stochastic submodels, which allows model projections to incorporate uncertainty in model inputs. Within its framework, the **CCE** accounts for a variety of ungulate stressors including land disturbance, environmental conditions, harvest and natural mortality. These stressors are integrated into several interconnected submodels which make up the **CCE** modeling framework; a *Vegetation/Land Use Change submodel*, which forecasts changes in vegetation and land use across a herd’s range over space and time; a *Caribou Movement submodel*, which projects daily movement of individual caribou across the study area based on historical data; a *Body Condition submodel*, which projects the body condition of an individual caribou cow (and its newborn calf) as it moves across the range over the course of a single year; and finally, a *Population submodel*, adapted from the [DG-Sim](https://apexrms.github.io/dgsim/){:target="_blank"} stochastic population model, which operates on an annual time step, projecting the total size of the herd’s population (by sex and age) each year as a function of the current population size, birth rates, natural mortality rates, and harvest levels. The submodels which make up the **CCE** framework originate from pre-existing published models, all of which contain source code that is freely available.

<a href="http://www.youtube.com/watch?v=eYjAEqdovJM" target="_blank"><img src="assets/images/video-screencap.png" alt="Caribou Cumulative Effects Overview" align="right" style="padding: 15px" width="400" /></a>

To learn more about the **Carce** package and the **CCE** modeling framework, visit [http://www.apexrms.com/cce-barren-ground-caribou/](http://www.apexrms.com/cce-barren-ground-caribou/){:target="_blank"}.


## Requirements

This package requires SyncroSim [version 2.0.42](https://syncrosim.com/download/){:target="_blank"}.
<br>
<br>
## How to Install

For more information on **Carce**, including how to install, see the [Getting Started](https://apexrms.github.io/carce/getting_started.html){:target="_blank"} page.
<br>
<br>
## Links

Browse source code at
[http://github.com/ApexRMS/carce/](http://github.com/ApexRMS/carce/){:target="_blank"}
<br>
Report a bug at
[http://github.com/ApexRMS/carce/issues](http://github.com/ApexRMS/carce/issues){:target="_blank"}
<br>
<br>
## Developers

Leonardo Frid (Author, maintainer) <a href="https://orcid.org/0000-0002-5489-2337" target="_blank"><img align="middle" style="padding: 0.5px" width="17" src="assets/images/ORCID.png"></a>
<br>
Colin Daniel (Author)
<br>
Alex Embrey (Author)
<br>
Tom Roe (Author)
<br>
Robert White (Author)
<br>
Don Russell (Author)
