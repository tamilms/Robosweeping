using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Util;

namespace OfficeSweeper.Droid
{
	[Activity(Label = "OfficeSweeper", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		RelativeLayout rootLayout;
		ImageView img_sweeper;
		RadioGroup selectDirection;
		Button btn_cmdClean,btn_move;
		int MaxNumberOfCommand;
		float Current_XPosition, Current_YPosition;
		int screenHight, screenWidth;
		int CurrentCmdCount = 0;
		EditText edt_StepValue;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);


			SetContentView(Resource.Layout.Main);

			try
			{
			   initialization();


				//Button click event for Clean Command
				btn_cmdClean.Click += delegate
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
						// Reached Maximum number of command then alert give it user to task completed
						  ShowMessage("Task Completed", "Cleaned:" + MaxNumberOfCommand);
					  }
				  };


				// Command give to Robo into particular direction to move
				btn_move.Click += delegate
				{

					Current_XPosition = img_sweeper.GetX();
					Current_YPosition = img_sweeper.GetY();

					if (edt_StepValue.Text + "" != "")
					{
						//Get number of step moved into direction
						int stepValueToMove = Convert.ToInt32(edt_StepValue.Text + "");


						int selectedId = selectDirection.CheckedRadioButtonId;

						// find the radiobutton by returned id
						RadioButton radioButton = FindViewById<RadioButton>(selectedId);

						//from radio button group get selected radio button direction
						string myDirection = radioButton.Text + "";

						//call Robo to move into specific Direction
						CommandRoboToMoveSpecificDirection(myDirection, stepValueToMove);
					}
					else
					{
						ShowValidationMessage("Please enter the Step Value");
					}

				};
			}
			catch (Exception ex)
			{
			}
		}

		// Initialize basic components
		void initialization()
		{
			try
			{
				btn_cmdClean = FindViewById<Button>(Resource.Id.btn_sweep);
				btn_move = FindViewById<Button>(Resource.Id.btn_move);
				rootLayout = FindViewById<RelativeLayout>(Resource.Id.rootRelative);
				img_sweeper = FindViewById<ImageView>(Resource.Id.img_sweeper);
				img_sweeper.SetImageResource(Resource.Mipmap.ic_robot);
				selectDirection = FindViewById<RadioGroup>(Resource.Id.Rg_direction);
				edt_StepValue = FindViewById<EditText>(Resource.Id.edt_StepValue);

				//get screen width and height in pixel
				Screen_WidthHeightDensity();

				//Call web service for getting number of command and starting position
				setIntialPositionOfRobo();

			}
			catch (Exception ex)
			{ 
			}
		}

		//Robo move into specifice direction based on the instruction from user
		void CommandRoboToMoveSpecificDirection(String myDirection,int stepvalue)
		{
			try
			{
				if (myDirection.ToUpper() == "NORTH")
				{
					if (CheckOutOfScreen(stepvalue, 0, 1) == true)
						img_sweeper.Animate().X(Current_XPosition - stepvalue);
					else
						ShowValidationMessage("Current value go out side of the screen");


				}

				else if (myDirection.ToUpper() == "SOUTH")
				{
					if (CheckOutOfScreen(stepvalue, 0, 0) == true)
						img_sweeper.Animate().X(Current_XPosition + stepvalue);
					else
						ShowValidationMessage("Current value go out side of the screen");

				}

				else if (myDirection.ToUpper() == "EAST")
				{
					if (CheckOutOfScreen(stepvalue, 1, 0) == true)
						img_sweeper.Animate().Y(Current_YPosition + stepvalue);
					else
						ShowValidationMessage("Current value go out side of the screen");

				}
				else if (myDirection.ToUpper() == "WEST")
				{
					if (CheckOutOfScreen(stepvalue, 1, 1) == true)
						img_sweeper.Animate().Y(Current_YPosition - stepvalue);
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

				if ((xPosition <= 0 || xPosition >= screenWidth - img_sweeper.Width) || (yPosition <= 0 || yPosition >= screenHight - img_sweeper.Height))
				{

					return false;
				}
			}
			catch (Exception ex)
			{
			}
			return true;
		}

		// Display Validation Message
		public void ShowValidationMessage(string alertMessage)
		{
			Toast.MakeText(this,alertMessage,ToastLength.Short).Show();
		}

		// Display Success/Completed message
		void ShowMessage(string Title,String Message)
		{
			try
			{
				AlertDialog.Builder alert = new AlertDialog.Builder(this);
				alert.SetTitle(Title);
				alert.SetMessage(Message);
				alert.SetPositiveButton("Restart", (senderAlert, args) =>
				{
					MaxNumberOfCommand = 0;
					Current_XPosition = 0;
					Current_YPosition = 0;
					CurrentCmdCount = 0;

					selectDirection.Check(0);
					edt_StepValue.Text = "";
					setIntialPositionOfRobo();
				});

				alert.SetNegativeButton("Close", (senderAlert, args) =>
				{

				});

				Dialog dialog = alert.Create();
				dialog.Show();
			}
			catch (Exception ex)
			{
			}
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
			{
			}
		}

		// Get The Device Height and Width in Pixel
		void Screen_WidthHeightDensity()
		{
			try
			{
				DisplayMetrics displaymetrics = new DisplayMetrics();
				WindowManager.DefaultDisplay.GetMetrics(displaymetrics);
				screenHight = displaymetrics.HeightPixels;
				screenWidth = displaymetrics.WidthPixels;
			}
			catch (Exception ex)
			{
			}
		}


		//set the Image at Starting Position
		private void placeImageAtInitianPosition(float startX, float startY,ImageView image)
		{
			try
			{
				Current_YPosition = startX;
				Current_YPosition = startY;


				//check if the view out of screen

				if ((Current_XPosition <= 0 || Current_XPosition >= screenWidth - image.Width) || (Current_YPosition <= 0 || Current_YPosition >= screenHight - image.Height))
				{
					setIntialPositionOfRobo();
					ShowValidationMessage("Intial Start X and Y values goes out the screen,Again I check the Position");
					return;
				}

				image.SetX(Current_XPosition);
				image.SetY(Current_YPosition);
				image.Left = 10;
				image.Right = 10;
			}
			catch (Exception ex)
			{
			}

		}


	}
}

