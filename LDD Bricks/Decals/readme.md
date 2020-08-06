Decal images needs to be square.

The file name must contain only numbers.
LDD will crash if the name starts with letters and will only consider the numeric parts.
So two files starting with the same numbers but with different letters at the end will ovewrite themselves.

The maximum (safe) file name length is 6 digits. 
More digits works in the decorationMappings.xml but crash LDD when in a palette.