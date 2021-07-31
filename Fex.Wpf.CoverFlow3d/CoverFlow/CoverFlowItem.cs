using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace Fex.Wpf.CoverFlow3d.CoverFlow
{
    public class CoverFlowItem
    {
        /// <summary>
        /// Gets the visual for this item
        /// </summary>
        private FrameworkElement visual;

        /// <summary>
        /// Represents the Visual3D for this item
        /// </summary>
        public Viewport2DVisual3D Visual3d { get; private set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public double TargetPositionX { get; set; }
        public double TargetRotationY { get; set; }
        public double TargetScale { get; set; }
        public double TargetZIndex { get; set; }

        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double RotationY { get; set; }
        public double Scale { get; set; }
        public double ZIndex { get; set; }

        private QuaternionRotation3D rotation = new QuaternionRotation3D();

        /// <summary>
        /// Initializes the control item
        /// </summary>
        /// <param name="visual"></param>
        public CoverFlowItem(FrameworkElement visual)
        {
            // Create the visual3D component
            this.CreateVisual3D(visual);
        }

        /// <summary>
        /// Updates the render transformations for this item
        /// </summary>
        public void UpdateTransformations()
        {
            // Determine width and height
            var newWidth = this.Width / 100;
            var newHeight = this.Height / 100;

            this.PositionX += (this.TargetPositionX - this.PositionX) / 10;
            this.RotationY += (this.TargetRotationY - this.RotationY) / 10;
            this.Scale += (this.TargetScale - this.Scale) / 10;
            this.ZIndex += (this.TargetZIndex - this.ZIndex) / 5;

            rotation.Quaternion = new Quaternion(new Vector3D(0, 1, 0), this.RotationY);
            // Create the transforms
            this.Visual3d.Transform = new Transform3DGroup()
            {
                Children =
                    {
                        new ScaleTransform3D(newWidth * this.Scale, newHeight * this.Scale, 0),
                        new RotateTransform3D(rotation),
                        new TranslateTransform3D(this.PositionX, 0, this.ZIndex),
                    }
            };

        }

        /// <summary>
        /// Creates a 3D visual for the given visual item
        /// </summary>
        /// <param name="visual"></param>
        private void CreateVisual3D(FrameworkElement visual)
        {
            this.visual = visual;
            this.Width = visual.Width;
            this.Height = visual.Height;

            this.Visual3d = new Viewport2DVisual3D()
            {
                Geometry = this.CreateQuad(),
                Material = this.CreateVisualMaterial(),
                Visual = visual
            };

            RenderOptions.SetCacheInvalidationThresholdMinimum(this.Visual3d, 0.5);
            RenderOptions.SetCacheInvalidationThresholdMaximum(this.Visual3d, 2);
            RenderOptions.SetCachingHint(this.Visual3d, CachingHint.Cache);
        }

        /// <summary>
        /// Creates a material for a visual component
        /// </summary>
        /// <returns></returns>
        private Material CreateVisualMaterial()
        {
            Material frontMaterial = new DiffuseMaterial(Brushes.White);
            frontMaterial.SetValue(Viewport2DVisual3D.IsVisualHostMaterialProperty, true);
            return frontMaterial;
        }

        /// <summary>
        /// Creates a MeshGeometry3D that represents a Quad in 3D space
        /// </summary>
        /// <returns></returns>
        private MeshGeometry3D CreateQuad()
        {
            // quad
            Point3D[] _mesh = new Point3D[] { new Point3D(-1, 1, 0), new Point3D(-1, -1, 0), new Point3D(1, -1, 0), new Point3D(1, 1, 0) };
            Point[] _texCoords = new Point[] { new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 0) };
            int[] _indices = new int[] { 0, 1, 2, 0, 2, 3 };

            MeshGeometry3D simpleQuad = new MeshGeometry3D()
            {
                Positions = new Point3DCollection(_mesh),
                TextureCoordinates = new PointCollection(_texCoords),
                TriangleIndices = new Int32Collection(_indices)
            };

            return simpleQuad;
        }
    }

}
