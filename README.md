### Future ToDo : 

1. move all UI from the level to GameplayEssentialsScene for reuse in other levels (to avoid duplicates).
2. redo existing background camera animations - use one camera instead of several.
3. optimize import settings for all level assets.
4. implement updating of wins/defeats of specific horses and serialize this data into json for loading on game restart.
5. update audio management logic - implement AudioSource pooling for reuse instead of creating new ones for each object that can produce 3D sounds. 
6. realize the logic of game pause.
7. implement animations for UI through tweeners.
8. add Idle animation for horse model.
~~9. add indicators for horses during the race (with names or other type of indication)~~
10. configure Occlusion Culling for optimization
~~11. configure all Addressable groups and assets to avoid duplicates~~
12. Fix Addressable duplicated with default TMPro assets.

> **Note :** use custom *py.* script to export the model from Blender. *(+fix horse pivot point, it has z:0.5 offset from the center)*

![Horse_05](https://github.com/user-attachments/assets/fbfd99a3-dca5-4dcf-baec-86851fc8849d)
