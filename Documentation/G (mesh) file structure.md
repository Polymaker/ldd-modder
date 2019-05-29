# G File Structure
## File structure
* File header
* 3D Data
  * Vertices
    * Positions: List of Vector3
    * Normals: List of Vector3
    * UVs (if mesh is textured): List of Vector2
  * Triangles indices
* Round edge shader data (used for outlines on brick)
  * Mapping for each **index**
* Average normals (used by shader)
  * Mapping for each **index**
* Flex bone weights
  * Mapping for each **vertex**
* Stud culling information

### Header
The file starts with the following four characters: 10GB.
Here are the possible mesh types:
* Standard mesh (Hex: 0x3A Dec: 58)
* Textured mesh (Hex: 0x3B Dec: 59)
* Flexible mesh (Hex: 0x3E Dec: 62)
* Flexible textured mesh (Hex: 0x3F Dec: 63)

### 3D Data
The 3D data section contains the vertices positions and normals and texture coordinates if the mesh is textured.
**Positions**: A list of Vector3. Total size = [**Vertex Count**] \* 3 \* 4 bytes
**Normals**: A list of Vector3. Total size = [**Vertex Count**] \* 3 \* 4 bytes
**UVs**: A list of Vector2. Total size = [**Vertex Count**] \* 2 \* 4 bytes
**Triangles indices**: 

### Round edge shader data
This data is used by the shader to draw outlines on the bricks.

### Average normals
This data is used by the shader but I don't know for what purpose.

### Flex bone weights

### Stud culling information


## Data structures
### File header
Size | Data type | Description 
:------- | :---: | :--- 
 4 bytes | Char[4] | Header bytes (0x31 0x30 0x47 0x42) ASCII = '10GB'
 4 bytes | Int32 | **Vertex Count**
 4 bytes | Int32 | **Index Count**
 4 bytes | Int32 | **Mesh Type**
**Note:** The number of triangles is equals to **Index Count** / 3
### Vector3
Size | Data type | Description 
:------- | :---: | :--- 
 4 bytes | float | X
 4 bytes | float | Y
 4 bytes | float | Z
### Vector2
Size | Data type | Description 
:------- | :---: | :--- 
 4 bytes | float | X
 4 bytes | float | Y
### Bone Weight
Size | Data type | Description 
:------- | :---: | :--- 
 4 bytes | Int32 | Bone Id. References to the primitive xml FlexBone node.
 4 bytes | float | Weight