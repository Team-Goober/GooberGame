# Team Goobers Sprint 5

## Team Members:
- Pavan Rauch
- Bill Yang
- Yash Wani
- Minsoo Kim
- Luke Haynes
- Sam Espanioly

## Program Controls
- WASD to move main player
- R to reset the game
- Q to quit the game
- Z to use slot A
- X to use slot B
- I to open inventory, Can Press Z or X to equip selected item.
- Z to select item from inventory
- C to delete item from inventory
- Up, Down, Left, Right and WASD controls inventory position  
- M1/M2 click to change rooms sequentially
- M3 click on a door to go into that door directly

## Code Smells checked for:
- Shotgun Surgery
- Duplicate Code
- Dead Code
- Data Clumps
- Long Parameter List
- Inappropriate Intamacy, and Variable Naming
- Missing Method Headers
- Temporary Field
- Large Class
- Large Method
- Excessive or Unclear Comments
- File Organization
- Artificial Stupidity 

## Known Bugs
- On some machines you have to write "dotnet restore" in the terminal to run the game
- Dragon collision box is weird
- Some enemies fly off the map when pushed
- Bottom rooms do not show up on the mini-map / compass
- When continue state is activated health is not updated to the HUD for some reason
- Movement is sometimes buggy - can charge speed if moving while stuck on a tile
- After dying and hitting sometimes player will keep walking without any button trigger
- Sometimes random room generator takes a long time to proceed

## Assumption
- All characters are pushable
- Player can now move diagonally
- Game only has one level
- Passive powerups are permanent
- Random room generator spawns a room per room-type - so about 16-18 rooms and upto 41 rooms
- AI is hard

## Extra Tools and Processes Used
- Adobe Photoshop for frame rotation
- Tiled website for assisted tile building
- MS Paint for spritess
- Youtube for inspiration and algorithm research

