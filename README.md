COMP 476 Programming Assignment 1 - George Mavroeidis

FINAL MARK: 75%
- Did not do well on Q2 and could not get the Toroidal distance to work
- Some other small things, but the movement AI is fine

Professor: Kaustubha Mendhurwar
Teacher Assistant: Daniel Rinaldi

The purpose of the program was to experiment and study different AI Movement behaviours and implement them in a kinematic and steering context.
Code uses base structure from Lab 02 (made by Daniel Rinaldi) for setting up the game loop, but the AI implementation was left for the student
to work on. 

In conjunction of the Theoretical parts, Question 1 and Question 2, the programming part contains 3 parts (R1, R2 and R3).

R1: I had set up the scene (A1_R1_R2) from the lab using the plane floor and AI agent prefabs that were already animated. The wrapping mechanic works, but
the pursuer in the tag game is not aware of it when pursuing a target. Basically, the pursuer does not know that once its target wraps around,
it won't consider it as finding a closer dstance, so I disable it when I want to test the behaviours properly. I tried using the Toroidal distance, but
with no success. Check GameManager.cs on the last function to see my attempt.

R2: The same scene (Lab 02), but Kinematic Arrive and Kinematic Flee are set up properly. At script AIAgent.cs at line 88, you will see the Interpolation
Change in Rotation with a t2t = Time.deltatime * 1000f

R3: This new scene (A1_R3) is simulation a tag game using AI movement behaviours. Steering Flee, Seek, Arrive, Wander and Stop are used.

Important properties of AI Agent:
- trackedTarget = Transform of the target it is tracking (another AI agent)
- _ePlayerState = defines state of Agent of an enum called EPlayerState (Frozen, Unfrozen, Tagged, Targeted, Rescuer)
- behaviourType = behaviour type of agent (Steering or Kinematic)
- function: SetMaterial(Material mat): function that changes colors of a list of colors from GameManager.cs
- list of materials/colors: 0: Red (Pursuer), 1: Green (Wanderer/Target), 2: Yellow (Frozen)

The Game:
- All AI start with green color, Unfrozen state and with a Wander.cs and LookWhereYouAreGoingBehaviour.cs
- On Start, a random pursuer is chosen, meaning the pursuer becomes Tagged, gets Seek and LookWhereYouAreGoingBehaviour.cs behaviours and becomes Red
- The pursuer picks the nearest tagret, which has a white cone on top of it to indicate that it's the pursuer's target. The target itself has the pursue
as a trackedTarget so it uses Flee.cs and FaceAway.cs to run away from the target. The Pursuer seeks it and runs a bit faster to catch up.
- Once the pursuer reaches its target within a certain distance, the trackedTarget turns yellow, becomes Frozen, destroys its FaceAway.cs and Flee.cs
components and gets Stop.cs, which is setting velocity to 0.
- Once frozen, the pursuer seeks another UNFROZEN (Green) target and runs to it. The recently frozen target seeks a closest UNFROZEN (Green) player
that is NOT targeted by the pursuer to unfreeze it. This rescuer becomes blue and gets Arrive.cs and LookWhereYouAreGoingBehaviour.cs to rescue the frozen
player.
- Once arrived, the frozen person becomes green and unfrozen and both of them get Wander.cs and stick with LookWhereYouAreGoingBehaviour.cs to become
part of the pursuer's target pool again.
- Once all targets have been frozen, the last frozen target becomes the pursuer and everyone else becomes an Unfrozen (green) player with Wander.cs and
LookWhereYouAreGoingBehaviour.cs

For more information, check source code, which has nice comments. For R3, start with focusing on AIManager.cs

CAMERA CONTROLS DURING RUNTIME:
Right click drag mouse to look around
Right click hold mouse + WASD to move
Right click hold mount + QE to ascend/descend
(Same controls as Lab 02)

George Mavroeidis
40065356
