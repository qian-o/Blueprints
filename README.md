# Blueprints

Cross‑platform, SkiaSharp‑powered blueprint/node editor control for .NET.

It provides a high‑performance, zoomable canvas with nodes, pins, and connections, suitable for graph editors, visual scripting, and data‑flow UIs.

## Features

- SkiaSharp rendering with high‑DPI support
- Pan/zoom with world/screen transforms
- Nodes, pins, and Bezier connections
- Drag‑and‑drop connection preview (pending connections)
- Hit testing and interaction events
- Rendering optimization via simple occlusion culling
- Theming support: Default theme with Light/Dark modes (Fluent and Cupertino planned)
- Works with WinUI 3 and Uno Platform

## Platforms and Rendering Backends

| Platform | Rendering Backend |
| :------: | :---------------- |
| WinUI 3  | SwapChainPanel    |
| Uno      | SKCanvasElement   |

## Getting Started

This repo includes example projects you can run directly:

- `sources/Examples/Example.WinUI`
- `sources/Examples/Example.Uno`

## Screenshot

![image](https://raw.githubusercontent.com/qian-o/Blueprints/master/images/1.png)
![image](https://raw.githubusercontent.com/qian-o/Blueprints/master/images/2.png)

## Notes
This project is in an early development stage. The public API is subject to change. Issues and pull requests are welcome.
