c-sharp-Tetris
==============

A work-in-progress version of Tetris made in c# - primarily to test out eventhandlers and basic graphics.
This still has some bugs, like pieces getting stuck to the right wall, and an occasional glitch with pieces not properly being counted as already placed for purposes of collision.  Each time a piece is moved, it will fall one line as I have not implemented a timer yet.  Sometimes there is also a glitch when a line is completed, and the above lines do not fall down exactly as intended.  This will be fixed when I have time.

The next shape coming is displayed for you in a side box.  Black shapes are active and blue shapes are placed.

The controls are:

Left Arrow Key - Move left, 
Right Arrow Key - Move right, 
Up Arrow Key - Rotate counter-clockwise, 
Down Arrow Key - Rotate clockwise, 
Space Bar - Drop


Controllers:
GameLogic.cs - logic for the game, implements ISquareObserver, 
Start.cs - with the void Main


Models:
ISquareObserver.cs - observer interface for altering the view, 
SquareFilledEventArgs - subclass of EventArgs for passing information


Views:
SimpleGraphicsForm.cs (partial class) - subclass of Form, with methods for bitmap drawing


GameLogic calls on SimpleGraphicsForm (though the interface) to fill squares.  Each time a square is filled, SimpleGraphicsForm fires an event.  GameLogic receives the event, and updates it's array to keep track of the game board (where the active piece is, and where already filled squares are).

Basically, if there wasn't a collision, the piece is erased and then redrawn in a new position.  If there was a collision, the piece is instead drawn in blue as 'placed' at it's current position.  If a row is completed (entirely blue), it is taken away and everything above falls down one line. 
