# Descend Below

A rogue-like video game where you combat enemies to progress through endless randomly generated floors. Grab weapons, spells, and potions to prepare yourself for the descent below.

## Instructions

To run the game, you need .NET version 7 or higher installed on your machine. After cloning this repository and `cd`ing into its directory, run `dotnet run` in the terminal to start the game.

W/A/S/D to move.  
Left click to attack.  
Right click to interact with objects like chests or staircases. Note that you must get close to these objects to interact with them. There should be a blinking indicator when you are close enough.  
Q to consume a healing potion. The number of charges you have must not be 0.  
E to cast your equipped spell. Note that you do not start the game with a spell and must find one before first. Spells cannot be cast when on cooldown.  
ESC to pause/unpause the game.  

## Demonstration video

![descend below](https://github.com/user-attachments/assets/b03dd5eb-ef48-4020-bd16-35da1bee04bf)

## Design

### Class diagram (showing 3 Design Patterns highlighted in red)

![UML Class Diagram](https://github.com/user-attachments/assets/2a4dd379-7776-44dd-af99-3c97dd0618ad)

### Game loop sequence diagram

![UML Sequence Diagram - Game Loop](https://github.com/user-attachments/assets/8c186a14-1a9c-44ab-a656-1f58df74a995)

### Collision handler sequence diagram

![UML Sequence Diagram - HandleCollisions](https://github.com/user-attachments/assets/93ef4e3a-07c9-43a4-962a-94a626343e79)

### Chest Interaction handler sequence diagram

![UML Sequence Diagram - HandleChestInteraction](https://github.com/user-attachments/assets/08f92d82-89f6-47b6-9b96-80f219ba08d2)
