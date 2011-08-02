using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ORTS.Core.OpenTKHelper
{
    public delegate void ShaderMessageEventHandler(string error);

    public enum ShaderProgramUniforms
    {
        ProjectionMatrix,
        ModelviewMatrix
    };

    public class ShaderProgram
    {
        private readonly Dictionary<ShaderProgramUniforms, int> _uniforms = new Dictionary<ShaderProgramUniforms, int>();
        private readonly Dictionary<ShaderType, int> _shaders = new Dictionary<ShaderType, int>();
        public int Program;

        public ShaderProgram()
        {
            Program = GL.CreateProgram();
        }

        public event ShaderMessageEventHandler OnMessage;

        public void AddUniform(ShaderProgramUniforms uniform)
        {
            Enable();
            _uniforms[uniform] = GL.GetUniformLocation(Program, uniform.ToString());
            if (OnMessage != null) OnMessage("Uniform Created :" + uniform.ToString() + " at: " + _uniforms[uniform]);
            Disable();
        }

        public void UpdateUniform(ShaderProgramUniforms uniform, ref Matrix4 matrix)
        {
            GL.UniformMatrix4(_uniforms[uniform], false, ref matrix);
            if (OnMessage != null) OnMessage("Uniform Updated :" + uniform.ToString());
        }

        public void AddShader(ShaderType type, String source)
        {
            _shaders[type] = GL.CreateShader(type);
            GL.ShaderSource(_shaders[type], source);
            GL.CompileShader(_shaders[type]);
            if (OnMessage != null) OnMessage(GL.GetShaderInfoLog(_shaders[type]));
            int compileResult;
            GL.GetShader(_shaders[type], ShaderParameter.CompileStatus, out compileResult);
            if (compileResult != 1)
            {
                if (OnMessage != null) OnMessage("Compile Error:" + type.ToString());
            }
            if (OnMessage != null) OnMessage("Compiled");
            GL.AttachShader(Program, _shaders[type]);
        }

        public void Link()
        {
            string info;
            GL.LinkProgram(Program);
            GL.GetProgramInfoLog(Program, out info);
            if (OnMessage != null) OnMessage(info);
            foreach (var kvp in _shaders)
            {
                GL.DeleteShader(kvp.Value);
            }
        }

        public void Enable()
        {
            GL.UseProgram(Program);
        }

        public void Disable()
        {
            GL.UseProgram(0);
        }
    }
}