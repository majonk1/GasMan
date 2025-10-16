Trello: https://trello.com/invite/b/68d223373c421f39b8bdfdb2/ATTIce2e856996b54d935016b3a34fb7299dE34E332D/my-trello-board


Sam Bib:
Inventory: https://www.youtube.com/watch?v=pmBv0Cagx_o&ab_channel=GameDevBeginner
UI: https://www.youtube.com/watch?v=1OwQflHq5kg&t=34s&ab_channel=ChristinaCreatesGames
Audio Manager: https://www.youtube.com/watch?v=rdX7nhH6jdM&ab_channel=RehopeGames

Controls:
A D to move
R to restart the level
Tab inventory
Mouse to interact with inventory
Alt f4 to close game

We had a few issues with github forking. The main contains a test scene with no levels, just the sound working. And Liam's branch  has the levels but with no sound.
Sorry.

/////
Game Mechanics Summary:
/////

You are a spaceman in a zero gravity spaceship that has had various noble gases leak across the spaceship.
Based on what gases you have equipped in your inventory, you will float either upwards or downwards to the respective layer of gas.
This is determined by the average buoyancy of gases equipped, rounded to the nearest discrete whole number.
The player is able to move left or right throughout the level, and must change equipped gases to float up or down, by picking them up or discarding them using their inventory.
The game is a puzzle game, with the goal being to get to the end of the level.
Be careful though, your spacesuit is pressurized and will burst if the player bumps their head or feet into a platform (due to floating up or down into it), causing you to reset the level.
Plan ahead accordingly.


/////
Level Design & Balancing:
/////

After playtesting the first paper prototype level for the game, it was found that the level was much more complex and difficult than anticipated, and another simpler level was made.

After further feedback from both throughout the group and the team, it was decided that the concept worked well, and could be expanded upon, and that the level had a good amount of difficulty, with some optional difficulty for players going for the optional objective.


For the week 6 prototype, the paper prototype level was digitized. The feedback from the playtest found that players liked the concept and thought it had a good amount of challenge to it, but found it a bit hard to grasp the concept at first, but would adapt to the mechanics well after spending time with them.
From this it was concluded that the game should include another easier level, to function as a tutorial that can teach the player the mechanics and help streamline the learning process for the game.

As such for the week 7 submission, the week 6 prototype level was remade, and a tutorial level created to help with the new player experience.

For the tutorial level, the text is intended to help the player understand some of the mechanics more easily.
For the second level, the platforms are colour coded based on the layer they are in, in order to clearly portray which discrete layer they fall within.

For an actual implementation of the game, the tutorial would be more interactive, and not just have floating text.
