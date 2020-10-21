# OhSpider
 This is my own personal try on procedural kinematics 
 
A couple things that really need to change:
First and foremost: I used examples for the leg turning, this could be done differently but it works well, credit to this video for showing me how to do it https://youtu.be/qqOAzn05fvk

The way the legs are stuck to the ground should be different, right now I'm using an extra gameobject and script for what could be done with less and better code.
The way the raycasts are happening is unperformant and should be changed, gravity shouldn't be a thing in them since we want the legs to be placed on vertical surfaces as well

The rotation of the body itself feels and looks jittery, not really sure what I'm doing wrong there just yet but more info to be followed
The way calculating the distance the leg has to step to should be changed as well, right now there are moments the leg can step towards a position that makes the leg clip through surfaces.

While rotating the legs aren't moving as correctly as they should, meaning they stay in the position and don't keep a normal stepping pattern, should also be changed.

Better camera behavior selfimplemented, right now Cinemachine was saving me but no excuses
