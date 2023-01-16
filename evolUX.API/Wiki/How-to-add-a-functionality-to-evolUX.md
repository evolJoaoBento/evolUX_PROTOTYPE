# Introduction
If you need to add a functionality to evolUX there are many ways to do so, depending on what you are trying to implement. However generically for most substantial functions you will need to follow a particular structure based on the MVC and UI/API pipeline and based on the SOLID principles. Here we will try to give an example and explain superficially and exemplify how to follow this structure and implement what you need where you need it for the most common approach for evolUX development. 
# Structure Overview
The evolUX design structure can conceptually be thought in the following way:
![image.png](/.attachments/image-5949dfdf-7777-4982-ab7a-e87f08432e45.png)
The <span style="color:lightblue"><b>*API*</b></span> and the <span style="color:orange"><b>*UI*</b></span> are both MVC projects therefore both follow the MVC principle of Model-View-Controller
</p>
The previous image was a more simplified version of the structure to assist in understanding the structure conceptually. However the following is a more accurate and descriptive representation of the used structure.

![image.png](/.attachments/image-541192fe-c8d7-484e-9159-b40aacad68d6.png)
The grey squares represent the interfaces for the respective controllers therefore following the dependency inversion principle of the SOLID principles.
Each dashed line box represents a separate project that are part of the same solutions, however they can be separate (one API on the server machine and various UI on the client machines).
One thing to always keep in mind is that the models are shared between both the <span style="color:lightblue"><b>API ("blue dashed box")</b></span> and the <span style="color:orange"><b>UI ("orange dashed box")</b></span>
