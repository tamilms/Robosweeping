// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace OfficeSweeper.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIButton Button { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btn_cmdClean { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btn_east { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btn_move { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btn_north { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btn_south { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btn_west { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField edt_StepValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img_sweeper { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btn_cmdClean != null) {
                btn_cmdClean.Dispose ();
                btn_cmdClean = null;
            }

            if (btn_east != null) {
                btn_east.Dispose ();
                btn_east = null;
            }

            if (btn_move != null) {
                btn_move.Dispose ();
                btn_move = null;
            }

            if (btn_north != null) {
                btn_north.Dispose ();
                btn_north = null;
            }

            if (btn_south != null) {
                btn_south.Dispose ();
                btn_south = null;
            }

            if (btn_west != null) {
                btn_west.Dispose ();
                btn_west = null;
            }

            if (edt_StepValue != null) {
                edt_StepValue.Dispose ();
                edt_StepValue = null;
            }

            if (img_sweeper != null) {
                img_sweeper.Dispose ();
                img_sweeper = null;
            }
        }
    }
}