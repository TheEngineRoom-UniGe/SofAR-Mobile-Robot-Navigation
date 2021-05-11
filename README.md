# SofAR-Mobile-Robot-Navigation

UNITY INSTALLATION (WINDOWS)
1) Visit https://unity3d.com/get-unity/download and download Unity Hub
2) Through the Hub, install Unity 2020.2.2 (the version for which the projects have been developed) or later. Beware that later versions (eg. Unity 2021.1.x) may be incompatible with the projects


INSTALLATION AND SETUP
1) on UBUNTU:
	- Visit https://github.com/TheEngineRoom-UniGe/SofAR-Mobile-Robot-Navigation and download the ROS packages needed for the project
	- Extract the packages to your catkin_ws
	- Open the 'mobile_robot_navigation_project' package, then navigate to config folder and open the params.yaml file. Under ROS_IP, insert the IP address of your machine.
	- Compile packages with catkin_make
	- Once all dependencies are resolved, do: 'roslaunch mobile_robot_navigation_project navigation.launch' to start communication with Unity (ensure the server communication is up and running, you should see the following line in the terminal 'Starting server on YOUR IP:10000')
2) on WINDOWS:
	- Visit https://github.com/TheEngineRoom-UniGe/SofAR-Mobile-Robot-Navigation to download the Unity project folder, extract it, then open Unity Hub and ADD the project to your projects list using the associated button.
	- Finally, open the project
	- In the bar on top of the screen, open the 'Robotics/ROS settings' tab and replace the 'ROS IP Address' with the IP of the machine running ROS.
	- If the previous steps have been successful, you should be able to enter Editor mode (via the Play button) and play the simulation.
	- Il the communication is running correctly, on UBUNTU you can echo the topics that are being exchanged between ROS and Unity.
	- Happy navigation!

TIPS
1) If you experience a strange window when opening the Unity Project for the first time, simply do Quit, then reopen the project and it should not bother you further.
2) Should you experience issues in the communication between ROS and Unity, especially when publishing FROM ROS TO Unity, remember to disable the firewall of your PC, as it could interfere with the sending of messages over TCP.
3) When running your project, be sure to launch the ROS files BEFORE playing the Unity simulation, in order to have the server endpoint node up and running before initializing communication.
