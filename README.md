# Archon (WIP)
A X-Platform framework written in MonoGame to mirror the Libgdx asset pipeline. Making the creation of New games and the Porting of old games feel more intuitive.

## Core Features 
 These are features that are required to expedite the development process.
 
 - **Input Event Handling**: monogame only offers state polling which is good for most cases but does not cover every case. 
 - **TextureRegions&TextureAtlases**: Since we wont be using gl_textures to prevent batch flushing we should make heavy use of TextureAtlasing. Monogame only provides basic Textures that have the capability but no implementation.
 - **Tiled TMX Loading&Rendering**: The ability to load .tmx data and render it.
 - **SceneGraph**: Top-Down hierarchy with a root node that is able to recieve input events and pass them down to child nodes. Child nodes are drawn in parents coordinates. 
 - **Rich UX System**: Built on top of the Scenegraph with Nodes acting as buttons,labels, textboxes ect.
 - **Tweening Library**: Tweening library for interpolating node values.
 - **Particle System**: Able to load gdx-particle files, for easy creation with the libgdx particle editor.
 
 ## Extras
 - **Specialized Pool Collections**: memory can be an issue, So to avoid genereating garbage we use pools to avoid object instantiation.
 - **Console UI**: for quickly prototyping levels and testing parameters on the fly from within the game. 
 - **Node Systems**: Being able to add logic to a node independently will allow re-use of the same code for other nodes.(nodes can be used as a sort of layering system). Node Systems are able to act on child nodes(of who the system belongs to) when they are added,removed or updated ect. An example of this could be a YSorting System that just sorts all the child nodes using their y coordinate. 
 - **Asset Manager**: Used to make loading assets simple and prevent asset overloading. 
 
 ### Styling 
 
 - **Variable&Method Names**: `camelCase` descriptive names. 
 - **Class Name**: `CapitalizeWords`. Adding a `A_` To Classes Wrapping Monogame classes
 - **Brackets**: on their own line. 
 - **Comments**: just use `//` for single and multi line. Looks neat and its less hassle
 
### Notes: 
- Should not use Magic Data. If the same number or string is being used multiple times in a method lets put it on a variable. 
 

