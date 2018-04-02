using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Lab_3___Invaders
{
	class UserInterface
	{
		//To access them in Form1.cs
		public PictureBox menuImg;
		public PictureBox btnUnmuteImg;
		public PictureBox btnMuteImg;

		public UserInterface()
		{
			menuImg = new PictureBox();
			btnUnmuteImg = new PictureBox();
			btnMuteImg = new PictureBox();
		}

		public Panel CreatePanel(int no)
		{
			Panel panel = new Panel();
			PictureBox image = new PictureBox();
			Bitmap menuImage;

			switch (no)
			{
				case 1: //the panel is the main menu
					panel.Name = "mainMenu";
					panel.Location = new Point(0, 0);
					panel.Size = new Size(794, 672);

					image.Name = "menuImage";
					image.Location = new Point(0, 0);
					image.Size = new Size(794, 672);
					menuImage = Properties.Resources.main;
					image.Image = menuImage;

					PictureBox btnUnmute = new PictureBox();
					btnUnmute.Name = "btnUnmute";
					btnUnmute.Location = new Point(730, 15);
					btnUnmute.Size = new Size(56, 56);
					btnUnmute.BackColor = Color.Transparent;
					Bitmap unmuteImage = Properties.Resources.unmute;
					btnUnmute.Image = unmuteImage;
					//set the parent object of the picturebox as the background image so
					//the button background can be transparent
					image.Controls.Add(btnUnmute);
					//Assign into public field
					btnUnmuteImg = btnUnmute;

					PictureBox btnMute = new PictureBox();
					btnMute.Name = "btnMute";
					btnMute.Location = new Point(730, 15);
					btnMute.Size = new Size(56, 56);
					btnMute.BackColor = Color.Transparent;
					btnMute.Visible = false;
					Bitmap muteImage = Properties.Resources.mute;
					btnMute.Image = muteImage;
					//set the parent object of the picturebox as the background image so
					//the button background can be transparent
					image.Controls.Add(btnMute);
					//Assign into public field
					btnMuteImg = btnMute;

					break;

				case 2: //the panel is the acoreboard
					panel.Name = "scoreBoardMenu";
					panel.Location = new Point(0, 0);
					panel.Size = new Size(794, 672);

					image.Name = "menuImage";
					image.Location = new Point(0, 0);
					image.Size = new Size(794, 672);
					menuImage = Properties.Resources.scoreboardMenu;
					image.Image = menuImage;

					break;
			}

			//Add controls into the panel created
			panel.Controls.Add(image);

			//Assign into public field of this class
			menuImg = image;

			return panel;l
		}
	}
}
