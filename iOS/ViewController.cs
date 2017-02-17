using System;
using BigTed;
using CoreGraphics;
using UIKit;

namespace OfficeSweeper.iOS
{
	public partial class ViewController : UIViewController
	{
		int MaxNumberOfCommand;
		float Current_XPosition, Current_YPosition;
		int CurrentCmdCount = 0;
		nfloat screenHight, screenWidth;
		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			try
			{
				Initialization();

				//Button click event for Clean Command
				btn_cmdClean.TouchUpInside += delegate
				{
					// calculate the number of command not exit than the Maximum number of command
					if (CurrentCmdCount < MaxNumberOfCommand)
					{
						CurrentCmdCount++;
						if (CurrentCmdCount >= MaxNumberOfCommand)
						{
							//Reached Maximum number of command then alert give it user to task completed
							ShowMessage("Task Completed", "Cleaned:" + MaxNumberOfCommand);
						}
					}
					else
					{
						//Reached Maximum number of command then alert give it user to task completed
						ShowMessage("Task Completed", "Cleaned:" + MaxNumberOfCommand);
					}
				};


				// Command give to Robo into particular direction to move
				btn_move.TouchUpInside += delegate
				{
					Current_XPosition = (float)img_sweeper.Frame.X;
					Current_YPosition = (float)img_sweeper.Frame.Y;

					if (edt_StepValue.Text + "" != "")
					{
						//Get number of step moved into direction
						int stepValueToMove = Convert.ToInt32(edt_StepValue.Text + "");
						string myDirection = "";

						//from radio button group get selected radio button direction
						if (btn_north.Selected == true)
						{
							myDirection = btn_north.TitleLabel.Text + "";
						}
						else if (btn_south.Selected == true)
						{
							myDirection = btn_south.TitleLabel.Text + "";
						}
						else if (btn_east.Selected == true)
						{
							myDirection = btn_east.TitleLabel.Text + "";
						}
						else if (btn_west.Selected == true)
						{
							myDirection = btn_west.TitleLabel.Text + "";
						}
						CommandRoboToMoveSpecificDirection(myDirection, stepValueToMove);

					}
					else
					{
						ShowValidationMessage("Please enter the Step Value");
					}
					//img_sweeper.Frame = new CGRect(x: Current_XPosition+10, y: Current_YPosition+10, width: img_sweeper.Frame.Size.Width, height: img_sweeper.Frame.Size.Height);
				};
			}
			catch (Exception ex)
			{
			}

		}

		// Initialize basic components
		void Initialization()
		{
			try
			{
				//Get Screen Width and Height at runtime
				Screen_WidthHeightDensity();

				//Get Nomral button into Radio button
				setRadioButton();

				//Call web service for getting number of command and starting position
				setIntialPositionOfRobo();

				//Set Textfield accepts only numbers
				edt_StepValue.ShouldChangeCharacters = (textField, range, replacement) =>
				{
					int number;
					return replacement.Length == 0 || int.TryParse(replacement, out number);
				};
			}
			catch (Exception ex)
			{ }
		}


		// Set Normal button looks and work like Radio button Intial setup
		void setRadioButton()
		{
			try
			{
				btn_north.TouchUpInside += HandleTouchUpInside;
				btn_south.TouchUpInside += HandleTouchUpInside;
				btn_east.TouchUpInside += HandleTouchUpInside;
				btn_west.TouchUpInside += HandleTouchUpInside;
				btn_north.SetImage(UIImage.FromBundle("ic_checked"), UIControlState.Selected);
				btn_north.SetImage(UIImage.FromBundle("ic_unchecked"), UIControlState.Normal);
				btn_south.SetImage(UIImage.FromBundle("ic_checked"), UIControlState.Selected);
				btn_south.SetImage(UIImage.FromBundle("ic_unchecked"), UIControlState.Normal);
				btn_east.SetImage(UIImage.FromBundle("ic_checked"), UIControlState.Selected);
				btn_east.SetImage(UIImage.FromBundle("ic_unchecked"), UIControlState.Normal);
				btn_west.SetImage(UIImage.FromBundle("ic_checked"), UIControlState.Selected);
				btn_west.SetImage(UIImage.FromBundle("ic_unchecked"), UIControlState.Normal);
			}
			catch (Exception ex)
			{
			}
		}


		// Display Success/Completed message
		void ShowMessage(string Title, String Message)
		{
			try
			{
				var alert = new UIAlertView(Title, Message, null, "Restart", "Close");
				alert.Show();
				alert.Clicked += (object senders, UIButtonEventArgs eventArgs) =>
			  {
				  if (eventArgs.ButtonIndex == 0)
				  {
					  MaxNumberOfCommand = 0;
					  Current_XPosition = 0;
					  Current_YPosition = 0;
					  CurrentCmdCount = 0;

					  btn_north.Selected = true;
					  edt_StepValue.Text = "";
					  setIntialPositionOfRobo();
					  // do something if cance
				  }
				  else
				  {

				  }
			  };
			}
			catch (Exception ex)
			{
			}
		}

		// Radio button select event 
		void HandleTouchUpInside(object sender, EventArgs ea)
		{
			try
			{
				var btn = (UIButton)sender;
				string text = btn.TitleLabel.Text.ToString().ToUpper();
				switch (text)
				{
					case "NORTH":
						btn_north.Selected = true;
						btn_south.Selected = false;
						btn_east.Selected = false;
						btn_west.Selected = false;
						break;
					case "SOUTH":
						btn_north.Selected = false;
						btn_south.Selected = true;
						btn_east.Selected = false;
						btn_west.Selected = false;
						break;
					case "EAST":
						btn_north.Selected = false;
						btn_south.Selected = false;
						btn_east.Selected = true;
						btn_west.Selected = false;
						break;
					case "WEST":
						btn_north.Selected = false;
						btn_south.Selected = false;
						btn_east.Selected = false;
						btn_west.Selected = true;
						break;
				}
			}
			catch (Exception ex)
			{
			}
		}

		// Display Validation Message
		public void ShowValidationMessage(string alertMessage)
		{
			try
			{
				BTProgressHUD.ShowToast(alertMessage, true, 1000);
			}
			catch (Exception ex)
			{ }
		}

		// Get The Device Height and Width in Pixel
		void Screen_WidthHeightDensity()
		{
			try
			{
				screenHight = UIScreen.MainScreen.Bounds.Height;
				screenWidth = UIScreen.MainScreen.Bounds.Width;
			}
			catch (Exception ex)
			{ }
		}

		// Set The Initial position of the Row using Server JSON Data
		void setIntialPositionOfRobo()
		{
			try
			{
				//call webservice to get Json data from server
				var result = CallWebService.GetSweepingAreaDetails();
				if (result != null && result.isSuccess == true)
				{


					var cmd_Instruction = result.Data;
					MaxNumberOfCommand = cmd_Instruction.NumberOfCommands;
					Current_XPosition = cmd_Instruction.Start_XPositiom;
					Current_YPosition = cmd_Instruction.Start_YPositiom;

					//Place the robo at starting position
					placeImageAtInitianPosition(cmd_Instruction.Start_XPositiom, cmd_Instruction.Start_YPositiom, img_sweeper);
				}
				else
				{
					ShowValidationMessage(result.Messasge);
				}
				}
			catch (Exception ex)
			{ }
		}

		//Robo move into specifice direction based on the instruction from user
		void CommandRoboToMoveSpecificDirection(String myDirection, int stepvalue)
		{
			try
			{
				if (myDirection.ToUpper() == "NORTH")
				{
					if (CheckOutOfScreen(stepvalue, 0, 1) == true)

						img_sweeper.Frame = new CGRect(x: Current_XPosition - stepvalue, y: Current_YPosition, width: img_sweeper.Frame.Size.Width, height: img_sweeper.Frame.Size.Height);
					else
						ShowValidationMessage("Current value go out side of the screen");


				}

				else if (myDirection.ToUpper() == "SOUTH")
				{
					if (CheckOutOfScreen(stepvalue, 0, 0) == true)

						img_sweeper.Frame = new CGRect(x: Current_XPosition + stepvalue, y: Current_YPosition, width: img_sweeper.Frame.Size.Width, height: img_sweeper.Frame.Size.Height);
					else
						ShowValidationMessage("Current value go out side of the screen");

				}

				else if (myDirection.ToUpper() == "EAST")
				{
					if (CheckOutOfScreen(stepvalue, 1, 0) == true)

						img_sweeper.Frame = new CGRect(x: Current_XPosition, y: Current_YPosition + stepvalue, width: img_sweeper.Frame.Size.Width, height: img_sweeper.Frame.Size.Height);
					else
						ShowValidationMessage("Current value go out side of the screen");

				}
				else if (myDirection.ToUpper() == "WEST")
				{
					if (CheckOutOfScreen(stepvalue, 1, 1) == true)

						img_sweeper.Frame = new CGRect(x: Current_XPosition, y: Current_YPosition - stepvalue, width: img_sweeper.Frame.Size.Width, height: img_sweeper.Frame.Size.Height);
					else
						ShowValidationMessage("Current value go out side of the screen");

				}
			}
			catch (Exception ex)
			{
			}
		}

		// Validation for Robo move into specific direction not going out of screen 
		bool CheckOutOfScreen(int numberOfStepsMoved, int direction, int arthimaticSymol)
		{
			try
			{
				int xPosition = 0, yPosition = 0;
				if (direction == 0)
				{
					if (arthimaticSymol == 0)
					{
						xPosition = (int)(Current_XPosition + numberOfStepsMoved);
					}
					else
					{
						xPosition = (int)(Current_XPosition - numberOfStepsMoved);
					}
					yPosition = (int)Current_YPosition;
				}
				else
				{
					if (arthimaticSymol == 0)
					{
						yPosition = (int)(Current_YPosition + numberOfStepsMoved);
					}
					else
					{
						yPosition = (int)(Current_YPosition - numberOfStepsMoved);
					}

					xPosition = (int)Current_XPosition;

				}

				if ((xPosition <= 0 || xPosition >= screenWidth - img_sweeper.Image.Size.Height) || (yPosition <= 0 || yPosition >= screenHight - img_sweeper.Image.Size.Height))
				{

					return false;
				}
			}
			catch (Exception ex)
			{ }
			return true;
		}


		//set the Image at Starting Position
		private void placeImageAtInitianPosition(float X, float Y, UIImageView image)
		{
			try
			{
				Current_XPosition = X;
				Current_YPosition = Y;


				//check if the view out of screen

				if ((Current_XPosition <= 0 || Current_XPosition >= screenWidth - image.Frame.Size.Width) || (Current_YPosition <= 0 || Current_YPosition >= screenHight - image.Frame.Size.Height))
				{
					setIntialPositionOfRobo();
					BTProgressHUD.ShowToast("Intial Start X and Y values goes out the screen,Again I check the Position", true, 1000);
					//Toast.MakeText(this, "Intial Start X and Y values goes out the screen,Again I check the Position", ToastLength.Short).Show();
					return;
				}
				image.Image = UIImage.FromBundle("ic_robot.png");

				image.Frame = new CoreGraphics.CGRect(x: Current_XPosition, y: Current_YPosition, width: img_sweeper.Frame.Size.Width, height: img_sweeper.Frame.Size.Height);
			}
			catch (Exception ex)
			{ }
		}
	}
}
