# Asteroids-Project-C-Programming-for-Unity-Game-Development-
_A C Programming Project for Unity Game Development (Coursera – University of Colorado)_

This is a space-themed shooter game project, built as part of my coursework for the “C Programming for Unity Game Development” specialization offered on Coursera by the University of Colorado. The goal of the project was to implement the classic Asteroids game in multiple iterations while learning game programming fundamentals and Unity integration.

## Course Link
[Programming for Unity Game Development Specialization — Coursera](https://www.coursera.org/specializations/programming-unity-game-development)

## Itch.io Link
(itch.io link)

## Table of Contents

- Project Iterations
- How to Run
- Features
- Credits

---

## Iteration 1: Rock of ages
- Start with an empty window. Don't worry, your first rock should appear after about a second
- Every second, if there are fewer than 3 rocks in the window, spawn a new rock, centered in the window, with a random sprite. You should use the random velocity code I provided to get the new rock moving, though you might need to change the MinImpulseForce and MaxImpulseForce values to make sure your rocks don't move too fast. In other words, make sure the resulting speed is slow enough that you get to see all 3 rocks in the window before any of them disappear
- When a rock leaves the window, it should destroy itself. It will be replaced by the functionality in the previous bullet within a second

## Iteration 2: Shipbuilding 101
- Let the player apply thrust using the space bar to move the ship. The thrust has to be applied in the direction the ship is facing, and the ship should accelerate appropriately based on physics
- Let the player rotate the ship using the left button to rotate counter-clockwise and the right button to rotate clockwise
- Screen wrap the ship. For example, when the ship leaves the bottom of the game window it should re-appear at the top of the game window. Make sure you handle all 4 edges of the game window as well as the corners, where you might have to wrap the ship both vertically and horizontally

## Iteration 3: Let it Rock
- Include a moving asteroid in your game that starts moving in a random direction with one of 3 random sprites
- Screen wrap the asteroid. For example, when the asteroid leaves the bottom of the game window it should re-appear at the top of the game window

## Iteration 4: Asteroid Spawning and Death
- Spawn 4 asteroids that move into the screen from the middle of the top, bottom, left, and right sides
- Make sure the asteroids don't collide with each other but do collide with the ship
- Destroy the ship when it collides with an asteroid

## Iteration 5: Let the shooting begin
- Have the ship shoot bullets in the direction the ship is facing
- Make sure the ship doesn't collide with bullets
- Make bullets destroy asteroids
- Make bullets destroy themselves after 2 seconds
- Make bullets screen wrap
- Make it so bullets don't collide with each other

## Iteration 6: The End of Asteroids
- Make bullets split asteroids into two smaller asteroids
- Make bullets destroy asteroids that are less than half the original size
- Add a game timer that displays how long the player has been playing
- Stop the game timer when the ship is destroyed
- Play sound effects when the player shoots a bullet, when a bullet collides with an asteroid, and when the ship is destroyed

## AFTER COURSE INSTRUCTIONS ENDED, I POLISHED THE GAME FEEL BY PLAYTESTING

## Iteration 7: 
- fixed movement feel by :
  - ship brakes after lifting off the movement button. There is no need to press S to brake.
  - ship now move backwards by pressing S
- balanced out asteroid spawning. Instead of spawning from all 4 directions, it now spawns from 2 directions, either up & down or left & right.
- added a fixed reticle with a fixed distance from a ship to help aim better.
---

## Features (Final Version)


---
## Credits

- Developed by: [Agniprabha Ganguly]
- Based on instructions from Coursera's University of Colorado specialization.
- Instructor: Dr. Tim "Dr. T" Chamillard
- Asteroid mechanics inspired by the original Atari Asteroids arcade game.

