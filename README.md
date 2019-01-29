# Rocket-Lanes

Rocket Lanes is a game project in Unity implementing both the client-server and the distributed network architectures for academic purposes.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 

### Prerequisites
What is needed to run the Unity engine and games can be found on their [website](https://unity3d.com/unity/system-requirements).

Keep in mind that the project was only tested on Windows 8 and 10. Since Unity is cross-platform, the game should be runnable on Linux and Mac.


### Installing

First, [download and install the Unity Engine version 2018.3.0b12](https://unity3d.com/get-unity/download/archive).
Then follow these steps:

```
- create blank project with Unity
- close Unity
- open project directory
- delete ProjectSettings, Packages and Temp folders
- open git bash in project folder
- $ git init
- $ git remote add origin
https://github.com/alexisdavidson/Rocket-Lanes.git
- $ git pull origin master
```

### Building

To create an executable, open the Unity project and under the File menu, select *Build Settings...* and finally press *Build*.

## Running the game

### The modes

The three first modes are: the single player mode, the client-server mode and the
distributed mode. The two last only exist for the sake of experimenting. They
simulate a player that joins and leaves games regularly, on the client-server or distributed network architecture.

### Settings

On the bottom of the screen settings can be tweaked. They only exist for the sake of experimenting.


With the *Quit after x mins* field you can set the software to leave the game after a specific
amount of time.

You can choose if you want to record network data when playing or not and set the name
of the subfolder to save these data in. Finally, you can trigger the mean computation of all
gathered data.

### Singleplayer
In the singleplayer mode, you play against 3 AIs. They move randomly and
send rockets and cast shield at regular intervals. Use the WASD keys to move and click on 
the buttons on the top of your lane (the most left one) to send rockets or shield.


### Client-server
To create a server, choose your port and click on *Host*. 
To join a server, enter the targeted IP and click on *Join*.

When in-game, at the top of the screen, there is an option to use an AI to control your character. 
It can be toggled anytime.


### Distributed
Enter your listening port on the corresponding field. 

When joining a game, enter the targeted port as well and click on *Join*. 
Otherwise, click on *New Game*.


### Automated client-server
Before launching this mode, you need to specify if the current instance will be a host or a client.
To do that, use the dropdown on the right of the *Automated Server-Client* button in the main menus.


### Automated distributed
Before launching this mode, you need to specify which listening port the current instance will have.
To do that, use the dropdown on the right of the *Automated Distributed* button in the main menus.

You also need to specify which range of ports it will try to join. If set to 1, it will only join the port 8881. 
If set to 7, it will join the ports between 8881 and 8887, randomly. 
It is meaningful to set this number to be the same as the number of instances running the experiment.

Finally, the option *Creator never leaves* makes is so that a creator of a game never leaves it. 
This is to simulate an experiment similar to the client-server one, where the host never leaves its game. 
To achieve this, on each instance, set this option to true and the range of ports to 1.

Here is an example of instructions to run an experiment with 3 instances:

* Launch three different instances of the game, place them meaningfully on your screen so you can see each clearly
* Set the *My Port* field to respectively 8881, 8882, 8883
* For each instance, set *Ports range to join* to 3
* Set *Creator never leaves* to false
* Click on *Automated Distributed* on each instance

With this setting, three instances will be creating, joining games and playing with each other.



## Built With

* [Unity Engine version 2018.3.0b12](https://unity3d.com/get-unity/download/archive)
* [UNet](https://docs.unity3d.com/Manual/UNet.html) - for client-server communication
* [Transport Layer API](https://docs.unity3d.com/Manual/UNetUsingTransport.html) - for distributed P2P communication

## Author

**Alexis Davidson**

## License

This project is licensed under the GNU General Public License v2.0 - see the [LICENSE](LICENSE) file for details.

