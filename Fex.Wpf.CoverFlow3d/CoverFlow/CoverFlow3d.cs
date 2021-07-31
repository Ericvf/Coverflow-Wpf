using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using iTunaFish.App.UserControls;
using iTunaFish.Library;

namespace Fex.Wpf.CoverFlow3d.CoverFlow
{
    public class CoverFlow3d : Grid
    {
        /// <summary>
        /// Represents the 3D viewport
        /// </summary>
        private Viewport3D viewport;

        private Dictionary<int, CoverFlowItem> coverFlowItems = new Dictionary<int, CoverFlowItem>();


        /// <summary>
        /// Represents the selected index
        /// </summary>
        private int selectedIndex = 0;

        /// <summary>
        /// Initializes the CoverFlow3d component.
        /// </summary>
        public CoverFlow3d()
        {
            // Init 3D stuff
            this.InitViewport();
            this.InitCamera();
            this.InitSurface();

            // Attach events
            this.Loaded += new RoutedEventHandler(CoverFlow3d_Loaded);
        }

        /// <summary>
        /// Initializes the timer responsible for rendering the frames
        /// </summary>
        private void InitTimer()
        {
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            this.UpdateTransformations();
        }

        /// <summary>
        /// Loads the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoverFlow3d_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate the children
            this.PopulateChildren();

            // Init timer
            this.InitTimer();

        }

        /// <summary>
        /// Initializes the viewport
        /// </summary>
        private void InitViewport()
        {
            this.viewport = new Viewport3D();
            this.Children.Add(this.viewport);
        }

        /// <summary>
        /// Initializes the camera
        /// </summary>
        private void InitCamera()
        {
            var position = new Point3D(0, 0, 15);
            var lookDirection = new Vector3D(0, 0, -1);
            var upDirection = new Vector3D(0, 1, 0);
            var fieldOfView = 90;

            this.viewport.Camera = new PerspectiveCamera(position, lookDirection, upDirection, fieldOfView);
        }

        /// <summary>
        /// Initializes the surface
        /// </summary>
        private void InitSurface()
        {
            Model3DGroup m3dGroup = new Model3DGroup() { Children = { new DirectionalLight(Colors.White, new Vector3D(0, 0, -1)) } };
            ModelVisual3D mv3d = new ModelVisual3D() { Content = m3dGroup };

            this.viewport.Children.Add(mv3d);
        }

        /// <summary>
        /// Populates the children in the control
        /// </summary>
        private void PopulateChildren()
        {
            this.UpdateTransformations();
        }

        /// <summary>
        /// Adds a child
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(FrameworkElement child)
        {
            var coverFlowItem = new CoverFlowItem(child);
            coverFlowItem.Scale = 1;
            coverFlowItem.TargetPositionX = 0;
            this.coverFlowItems.Add(this.coverFlowItems.Count, coverFlowItem);
            this.viewport.Children.Add(coverFlowItem.Visual3d);
        }

        /// <summary>
        /// Selects a child
        /// </summary>
        /// <param name="p"></param>
        public void SelectChild(int index)
        {
            var selectedChild = Math.Max(0, index);
            selectedChild = Math.Min(this.coverFlowItems.Count - 1, selectedChild);
            this.selectedIndex = selectedChild;
            this.UpdateTransformations();
        }

        /// <summary>
        /// Updates the transformations for all coverflow items
        /// </summary>
        private void UpdateTransformations()
        {
            foreach (var item in this.coverFlowItems)
            {
                int index = item.Key - this.selectedIndex;
                int skew = 1;

                if (index != 0)
                    skew = index > 0 ? -50 : 50;

                double targetOffsetX = 0;
                if (index != 0)
                {
                    targetOffsetX = index > 0
                            ? (index * 1.3) + 1
                            : (index * 1.3) - 1;
                }


                double targetScale = index == 0 ? 1 : 0.8;
                var coverFlowItem = item.Value;
                coverFlowItem.TargetPositionX = targetOffsetX;
                coverFlowItem.TargetRotationY = skew;
                coverFlowItem.TargetZIndex = index == 0 ? 4 : -Math.Abs(index) / 50;
                coverFlowItem.TargetScale = 0.8;
                coverFlowItem.UpdateTransformations();
            }
        }
        
        /// <summary>
        /// Selects the next child
        /// </summary>
        public void Next()
        {
            this.SelectChild(this.selectedIndex + 1);
        }

        /// <summary>
        /// Selects the previous child
        /// </summary>
        public void Prev()
        {
            this.SelectChild(this.selectedIndex - 1);
        }

        /// <summary>
        /// Selects the last child
        /// </summary>
        public void Last()
        {
            this.SelectChild(this.coverFlowItems.Count - 1);
        }

        /// <summary>
        /// Selects the first child
        /// </summary>
        public void First()
        {
            this.selectedIndex = 0;
        }

    }
}
