using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

namespace ORTS.Core.OpenTKHelper
{
    public delegate void ShaderMessageEventHandler(string error);
    public enum ShaderProgramUniforms { projection_matrix, modelview_matrix };

    public class ShaderProgram
    {
        public event ShaderMessageEventHandler OnMessage;
        public int shaderProgram;

        private Dictionary<ShaderType, int> shaders = new Dictionary<ShaderType, int>();
        private Dictionary<ShaderProgramUniforms, int> Uniforms = new Dictionary<ShaderProgramUniforms, int>();

        public ShaderProgram()
        {
            shaderProgram = GL.CreateProgram();
        }

        public void AddUniform(ShaderProgramUniforms uniform)
        {
            this.Enable();
            Uniforms[uniform] = GL.GetUniformLocation(shaderProgram, uniform.ToString());
            if (OnMessage != null) OnMessage("Uniform Created :" + uniform.ToString() + " at: " + Uniforms[uniform]);
            this.Disable();
        }
        public void UpdateUniform(ShaderProgramUniforms uniform,ref Matrix4 matrix)
        {
            
            GL.UniformMatrix4(Uniforms[uniform], false, ref matrix);
            if (OnMessage != null) OnMessage("Uniform Updated :" + uniform.ToString());
            
        }
        public void AddShader(ShaderType Type, String Source)
        {
            shaders[Type] = GL.CreateShader(Type);
            GL.ShaderSource(shaders[Type], Source);
            GL.CompileShader(shaders[Type]);
            if (OnMessage != null) OnMessage(GL.GetShaderInfoLog(shaders[Type]));
            int compileResult;
            GL.GetShader(shaders[Type], ShaderParameter.CompileStatus, out compileResult);
            if (compileResult != 1)
            {
                if (OnMessage != null) OnMessage("Compile Error:"+Type.ToString());
            }
            if (OnMessage != null) OnMessage("Compiled");
                GL.AttachShader(shaderProgram, shaders[Type]);
        }

        public void Link()
        {
            string info;
            GL.LinkProgram(shaderProgram);
            GL.GetProgramInfoLog(shaderProgram, out info);
            if (OnMessage != null) OnMessage(info);
            foreach (KeyValuePair<ShaderType, int> kvp in shaders)
            {
                GL.DeleteShader(kvp.Value);
            }
        }
        public void Enable()
        {
            GL.UseProgram(shaderProgram);
        }
        public void Disable()
        {
            GL.UseProgram(0);
        }
    }
}
