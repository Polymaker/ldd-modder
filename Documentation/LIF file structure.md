# LIF File Structure
## File structure
Here's an exemple of the structure of a LIF file:
* **File Header**
* **Root Block** (Block Type 1)
  * **File content** (Block Type 2)
  * **Root directory** (Block Type 3)
    * *File (Block Type 4)* 
    * *File (Block Type 4)* 
    * *Folder (Block Type 3)* 
   	  * *File (Block Type 4)* 
   	  * *File (Block Type 4)* 
   	  * *Folder (Block Type 3)* 
   		* *File (Block Type 4)* 
   		* *etc...*
  * **File hierarchy** (Block Type 5)
    * **Root directory** (Entry type 1)
      * *File (Entry type 2)*
      * *File (Entry type 2)*
      * *Directory (Entry type 1)*
   	    * *File (Entry type 2)*
   	    * *File (Entry type 2)*
   	    * *Directory (Entry type 1)*
   	  	  * *File (Entry type 2)*
   	  	  * *etc...*
		
## Data structures
***Note:** Most values are in big endian order*
### LIF Header
  SIZE   |  TYPE  |   DESCRIPTION
-------: | :----- | :-------------------------------
 4 bytes | Char[4]| Header (ASCI = 'LIFF')
 4 bytes |        | Spacing (Always equals 0)
 4 bytes | Int32  | Total file size (Int32 big endian)
 2 bytes | Int16  | Value "1" (Int16 big endian)
 4 bytes |        | Spacing (Always equals 0)
**Note:** The header is 18 bytes total.
### LIF Block
  SIZE   |  TYPE  |   DESCRIPTION
-------: | :----- | :-------------------------------
 2 bytes | Int16  | Block start/header (always 1)
 2 bytes | Int16  | Block type (1 to 5)
 4 bytes |        | Spacing (Always equals 0)
 4 bytes | Int32  | Block size in bytes (includes header and data)
 4 bytes |        | Spacing (Equals 1 for block types 2,4 and 5)
 4 bytes |        | Spacing (Always equals 0)
 X bytes |        | The block content/data.
The block type **1** is the "root block" and its size includes the remainder of the LIF file.
The block type **2** contains the files content/data. The block content seems hard-coded and it is always 1 (Int16) and 0 (Int32).
The block type **3** represents a folder. The block content is a hierarchy of type 3 and 4 blocks.
The block type **4** represents a file. The block data is the file content/data.
The block type **5** contains the files and folders names and some more information. The block content is a hierarchy of LIF entries.

**Note:** The block header's is 20 bytes total. The data size is equal to the specified size - 20 bytes.
### LIF Folder Entry
  SIZE   |  TYPE  |   DESCRIPTION
-------: | :----- | :-------------------------------
 2 bytes | Int16  | Entry type (equals 1)
 4 bytes | Int32  | Unknown value (equals 0 or 7) The value 0 seems to be used for the root folder.
 N bytes | Char[] | Folder name. (Unicode null-terminated text)
 4 bytes |        | Spacing (Always equals 0)
 4 bytes | Int32? | Spacing (Always equals 20)
 4 bytes | Int32  | The number of sub-entries (files and folders)
### LIF File Entry
  SIZE   |  TYPE  |   DESCRIPTION
-------: | :----- | :-------------------------------
 2 bytes | Int16  | Entry type (equals 2)
 4 bytes | Int32  | Spacing/unknown value (0 or 7)
 N bytes | Char[] | File name. (Unicode null-terminated text)
 4 bytes |        | Spacing (Always equals 0)
 4 bytes | Int32  | File size (it is actually the block size because it includes the block header size (20))
 8 bytes | Long (Filetime)  | Created, modified or accessed date
 8 bytes | Long (Filetime)  | Created, modified or accessed date
 8 bytes | Long (Filetime)  | Created, modified or accessed date