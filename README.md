
# 2D Vector Lights

2D real-time lights and shadow system for mobile devices. 
The system is well optimized and it's working great even on old mobile devices.



![image](https://drive.google.com/uc?export=view&id=1vn0CygLtsfWT1LTCIg6837OG7CSLWLk8)

## Features

- Real-Time
- Optimized
- Zero Garbage Collection
- Not using Raycast


## Limitations

- 8 Lights in range of each other
- May interfere with other shaders that uses Stencil Buffer
- Works with polygon collider only

## How it works

Basically creating a mesh (shadow) and discarding 
the pixels from the light mesh using Stencil Buffer
