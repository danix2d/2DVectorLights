
# 2D Vector Lights

2D real-time lights and shadow system for mobile devices. 
The system is well optimized and it's working great even on old mobile devices.



![image](https://danix2d.com/images/imgpriv/VectorLight/vectorlight.jpg)

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
