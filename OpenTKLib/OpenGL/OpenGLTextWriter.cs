using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using OpenTK.Graphics;



namespace OpenTKExtension
{
    public class OpenGLTextWriter : IDisposable
    {
        TextRendererAdapted textRenderer;
        private bool disposed;
        //TextRenderer textRenderer;

        Font serif = new Font(FontFamily.GenericSerif, 24);
        System.Drawing.Color backColor = Color.White;
        Brush textBrush = Brushes.Black;

       
        public OpenGLTextWriter()
        {
            textRenderer = new TextRendererAdapted(35, 35);
            //textRenderer = new TextRenderer(35, 35);

        }
        public static void DrawStringStatic(string text, double startX, double startY, double startZ)
        {
            TextRendererAdapted textRenderer = new TextRendererAdapted(35, 35);
            Font serif = new Font(FontFamily.GenericSerif, 24);
            System.Drawing.Color backColor = Color.WhiteSmoke;
            Brush textBrush = Brushes.Black;

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textRenderer.Texture);

            textRenderer.Clear(backColor);
            textRenderer.DrawString(text, serif, textBrush, new PointF(0.0f, 0.0f));

            double realHeight = 0.2f;
            double realWidth = 0.2f;

            GL.Begin(PrimitiveType.Quads);
            
            GL.TexCoord3(0.0f, 0.0f, 0f); GL.Vertex3(startX, startY, startZ);
            GL.TexCoord3(1.0f, 0.0f, 0f); GL.Vertex3(startX + realWidth, startY, startZ);
            GL.TexCoord3(1.0f, 1.0f, 0f); GL.Vertex3(startX + realWidth, startY + realHeight, startZ);
            GL.TexCoord3(0.0f, 1.0f, 0f); GL.Vertex3(startX, startY + realHeight, startZ);
            
            GL.End();

        }
        public void DrawString(string text, double startX, double startY, double startZ)
        {
            DrawString(text, startX, startY, startZ, backColor);


        }
        protected void DrawString(string text, double startX, double startY, double startZ, System.Drawing.Color mybackColor)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textRenderer.Texture);

            textRenderer.Clear(mybackColor);
            
            textRenderer.DrawString(text, serif, textBrush, new PointF(0.0f, 0.0f));


            double realHeight = 0.2f;
            double realWidth = 0.2f;

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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    textRenderer.Dispose();
                }


                // Note disposing has been done.
                disposed = true;

            }
        }
       
    }
}
