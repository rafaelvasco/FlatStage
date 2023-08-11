// using FlatStage.Foundation;

// namespace FlatStage;

// public class DepthStencilState
// {
//     public bool DepthBufferEnable { get; set; }

//     public bool DepthBufferWriteEnable { get; set; }

//     public bool StencilEnable { get; set; }

//     public CompareFunction DepthCompareFunction
//     {

//     }

//     public StencilCompareFunction StencilCompareFunction
//     {
//         get => _stencilCompareFunction;
//         set
//         {
//             if (_stencilCompareFunction != value)
//             {
//                 _stencilCompareFunction = value;

//             }
//         }
//     }

//     public StencilOperation StencilPass { get; set; }

//     public StencilOperation StencilFail { get; set; }

//     public uint StencilMask { get; set; } = 1;

//     public uint StencilWriteMask { get; set; } = 1;

//     private void UpdateDepthFlags()
//     {
//         _depthState = (Bgfx.StateFlags)_depthBufferFunction;
//     }

//     private void UpdateStencilFlags()
//     {
//         _stencilState = (Bgfx.StencilFlags)_stencilCompareFunction;

//         switch (_stencilPassOp)
//         {
//             case StencilOperation.Keep:
//                 _stencilState |= Bgfx.StencilFlags.OpPassZKeep;
//                 break;
//             case StencilOperation.Zero:
//                 _stencilState |= Bgfx.StencilFlags.OpPassZZero;
//                 break;
//             case StencilOperation.Replace:
//                 _stencilState |= Bgfx.StencilFlags.OpPassZReplace;
//                 break;
//             case StencilOperation.Increment:
//                 _stencilState |= Bgfx.StencilFlags.OpPassZIncr;
//                 break;
//             case StencilOperation.Decrement:
//                 _stencilState |= Bgfx.StencilFlags.OpPassZDecrsat;
//                 break;
//             case StencilOperation.IncrementSaturation:
//                 break;
//             case StencilOperation.DecrementSaturation:
//                 break;
//             case StencilOperation.Invert:
//                 break;
//         }
//     }

//     private CompareFunction _depthBufferFunction;
//     private StencilCompareFunction _stencilCompareFunction;
//     private StencilOperation _stencilPassOp;
//     private StencilOperation _stencilFailOp;

//     private Bgfx.StateFlags _depthState;
//     private Bgfx.StencilFlags _stencilState;
// }

