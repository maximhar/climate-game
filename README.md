# climate-game
A simulation of demographics, economy, climate change, and more, with a gaming element. Quite experimental so far.

What currently works:
* Demographics: realistic death rates, created from mortality data for the UK
* Demographics: trivial birth rates
* Economy: propensity to invest and propensity to spend 
* Economy: inflation rates based on supply/demand balance
* Economy: employment rate
* Economy: private debt
* Economy: government spending and government debt
* Government: adjustable government spending, allowing Keynesian interference

What's left (incomplete):
* General: implement multiple interacting regions (currently only 1 region is simulated)
* Government: adjustable monetary policy, allowing control of inflation
* Demographics: realistic birth rates based on economic factors
* Happiness: implement a happiness aspect that can be affected by the economic and government aspects
* Economy: implement trade between regions
* Energy: add an energy aspect, as an input to the economy aspect
* Resources: add an aspect representing physical resources, to act as an input to the economy and energy aspects
* Technology: add a technology aspect, potentially affecting a number of other aspects
* Environment: add an environment aspect, taking into account greenhouse emissions, deforestation, etc.
