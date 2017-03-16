using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using OpenTK.Graphics;



namespace OpenTK.Extension
{
    public class OpenGLTextWriter
    {
        TextRendererAdapted textRenderer;

        //TextRenderer textRenderer;

        Font serif = new Font(FontFamily.GenericSerif, 24);
        System.Drawing.Color backColor = Color.White;
        Brush textBrush = Brushes.Black;


        public OpenGLTextWriter()
        {
            textRenderer = new TextRendererAdapted(35, 35);
            //textRenderer = new TextRenderer(35, 35);

        }
        public static void DrawStringStatic(string text, float startX, float startY, float startZ)
        {
            TextRendererAdapted textRenderer = new TextRendererAdapted(35, 35);
            Font serif = new Font(FontFamily.GenericSerif, 24);
            System.Drawing.Color backColor = Color.WhiteSmoke;
            Brush textBrush = Brushes.Black;

            GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
            GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, textRenderer.Texture);

            textRenderer.Clear(backColor);
            textRenderer.DrawString(text, serif, textBrush, new PointF(0.0f, 0.0f));

            float realHeight = 0.2f;
            float realWidth = 0.2f;

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord3(0.0f, 0.0f, 0f); GL.Vertex3(startX, startY, startZ);
            GL.TexCoord3(1.0f, 0.0f, 0f); GL.Vertex3(startX + realWidth, startY, startZ);
            GL.TexCoord3(1.0f, 1.0f, 0f); GL.Vertex3(startX + realWidth, startY + realHeight, startZ);
            GL.TexCoord3(0.0f, 1.0f, 0f); GL.Vertex3(startX, startY + realHeight, startZ);

            GL.End();

        }
        public void DrawString(string text, float startX, float startY, float startZ)
        {
            DrawString(text, startX, startY, startZ, backColor);


        }
        protected void DrawString(string text, float startX, float startY, float startZ, System.Drawing.Color mybackColor)
        {
            GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
            GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, textRenderer.Texture);

            textRenderer.Clear(mybackColor);

            textRenderer.DrawString(text, serif, textBrush, new PointF(0.0f, 0.0f));


            float realHeight = 0.2f;
            float realWidth = 0.2f;

            GL.Begin(PrimitiveType.Quads);


            //if (startX == 0 && startY == 0)
            //{
            //    GL.TexCoord3(1.0f, 1.0f, 0f); GL.Vertex3(startX, startY, startZ);
            //    GL.TexCoord3(0.0f, 1.0f, 0f); GL.Vertex3(startX + realWidth, startY, startZ);
            //    GL.TexCoord3(0.0f, 0.0f, 0f); GL.Vertex3(startX + realWidth, startY + realHeight, startZ);
            //    GL.TexCoord3(1.0f, 0.0f, 0f); GL.Vertex3(startX, startY + realHeight, startZ);
            //}
            //else
            {
                GL.TexCoord3(0.0f, 0.0f, 0f); GL.Vertex3(startX, startY, startZ);
                GL.TexCoord3(1.0f, 0.0f, 0f); GL.Vertex3(startX + realWidth, startY, startZ);
                GL.TexCoord3(1.0f, 1.0f, 0f); GL.Vertex3(startX + realWidth, startY + realHeight, startZ);
                GL.TexCoord3(0.0f, 1.0f, 0f); GL.Vertex3(startX, startY + realHeight, startZ);

            }


            GL.End();


        }
    }
}
