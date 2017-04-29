using System;
using System.Collections.Generic;
using OpenTK.Graphics.ES20;
using Bool = OpenTK.Graphics.ES20.All;
namespace EW.Xna.Platforms.Graphics
{

    
    internal partial class Shader
    {
        private string _glslCode;//着色器源码

        //
        private int _shaderHandler = -1;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="isVertexShader"></param>
        /// <param name="shaderBytecode"></param>
        private void PlatformConstruct(bool isVertexShader,byte[] shaderBytecode)
        {
            _glslCode = System.Text.Encoding.ASCII.GetString(shaderBytecode);

            HashKey = EW.Xna.Platforms.Utilities.Hash.ComputeHash(shaderBytecode);
        }
        /// <summary>
        /// 获取着色器
        /// </summary>
        /// <returns></returns>
        internal int GetShaderHandle()
        {
            if (_shaderHandler != -1)
                return _shaderHandler;
            //创建着色器(顶点&片段)
            _shaderHandler = GL.CreateShader(Stage == ShaderStage.Vertex ? ShaderType.VertexShader : ShaderType.FragmentShader);
            GraphicsExtensions.CheckGLError();
            //着色器源码附加到着色器上
            GL.ShaderSource(_shaderHandler, _glslCode);
            GraphicsExtensions.CheckGLError();
            //编译着色器
            GL.CompileShader(_shaderHandler);
            GraphicsExtensions.CheckGLError();
            //定义一个整形变形标识编译是否成功
            int compiled = 0;
            GL.GetShader(_shaderHandler, ShaderParameter.CompileStatus, out compiled);
            GraphicsExtensions.CheckGLError();
            if(compiled != (int)Bool.True)
            {
                if (GL.IsShader(_shaderHandler))
                {
                    GL.DeleteShader(_shaderHandler);
                    GraphicsExtensions.CheckGLError();
                }
                _shaderHandler = -1;

                throw new InvalidOperationException("Shader Compilation Failed");
            }
            return _shaderHandler;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="usage"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal int GetAttribLocation(VertexElementUsage usage,int index)
        {
            for (int i =0; i < Attributes.Length; i++)
            {
                if((Attributes[i].usage == usage) && (Attributes[i].index == index))
                {
                    return Attributes[i].location;
                }
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="program"></param>
        internal void GetVertexAttributeLocations(int program)
        {
            for(int i = 0; i < Attributes.Length; i++)
            {
                Attributes[i].location = GL.GetAttribLocation(program, Attributes[i].name);
                GraphicsExtensions.CheckGLError();
            }
        }

        internal void ApplySamplerTextureUnits(int program)
        {
            foreach(var sampler in Samplers)
            {
                var loc = GL.GetUniformLocation(program, sampler.name);
                GraphicsExtensions.CheckGLError();
                if (loc != -1)
                {
                    GL.Uniform1(loc, sampler.textureSlot);
                    GraphicsExtensions.CheckGLError();
                }
            }
        }


        /// <summary>
        /// 重置平台上的图形设备
        /// </summary>
        private void PlatformGraphicsDeviceResetting()
        {
            if(_shaderHandler != -1)
            {
                if (GL.IsShader(_shaderHandler))
                {
                    GL.DeleteShader(_shaderHandler);
                    GraphicsExtensions.CheckGLError();
                }
                _shaderHandler = -1;
            }
        }



    }
}