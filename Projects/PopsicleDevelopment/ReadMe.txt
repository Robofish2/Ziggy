
Project install date: 3/12/12

Test

Steps to create a new install:
1) Make sure to compile the Debug and Release versions of the PopsicleImaging project.
2) Select the PopsicleImagingSetup project. Press F4 to bring up the properties page.
3) Increment the Version. Select Yes to update the Product Code.
4) Right click on the PopsicleImagingSetup project and selct Build.
5) The setup files will be located at: C:\CognexVisionPro\Projects\PopsicleImagingSetup\Release
6) Distribute both files in this directory for a target machine. To install, click on the Setup.exe file.

Other notes:

- The timerPictureTimer control is used to trigger the camera on a time basis. This will eventually be replaced
  by using an RS232 command sent from the PLC to request a vision trigger based on the scaled encoder pulse from the 
  qtrack board.
- The timerCheckForAllFormsClosed is used during a recipe change to ensure all windows have been closed. When this is
  determined, the recipe is changed over. The timer was needed to prevent a deadlock condition that existed when the
  recipe change was initiated before all forms were closed.
- Never run the PopsicleImaging.exe file from a different folder than where it was installed. Use a shortcut.
- If the development PC has NOT installed the application, the JobsPath is set the the local Jobs folder.
- If the development PC has the application installed the JobsPath is: 
  C:\Users\Public\BluePrint Robotics\PopsicleImaging\PopsicleImagingJobs\

Steps to add a numericUpDown control to a form:
1) Create a parameter in either the RuntimeParameters.cs or MachineConfig.cs file.
2) Drag a numericUpDown control on to the form and give it a unique name.
3) Access an existing numeric controls's event properties and look at the name in the 'ValueChanged' property.
4) Normally you would copy this name into the new numeric control's property if it is similar.
5) After doing step 4, double click on the new control and add the numeric control to the switch statement. There are
   lots of examples to look at.
6) Access the form load method and add the statement to initialize the numericUpDown control when the form is opened. Again,
   there are lots of examples to look at.
7) Beware that not all forms support parameters in the MachineConfig.cs file. Additional coding may be required
   to support the parameters.  

Shorthand for above procedure:
1) Add parameter to the RuntimeParameters.cs or MachineConfig.cs file. 
2) Add the contol to the page.
3) Update the 'ValueChanged' property.
4) Double click on control and add to case statement.
5) Add initializations in the form load methods.
   
      

