# CoStudy Cloud

CoStudy Cloud is a cloud-based Study Group Organizer designed to facilitate collaborative learning. Leveraging Google’s Cloud technology, this application aims to enable users to seamlessly join study groups, share learning resources, and schedule study sessions. 

## Key Features

- **User Authentication:** Users can authenticate using their Google accounts.
- **Role Assignment:** Users are assigned roles based on whitelist status.
- **User Profile Management:** Users can update their profile information and upload profile pictures.
- **Group Creation and Management:** Group Administrators can create study groups with unique titles and descriptions. Learners can join existing study groups.
- **Session Scheduling:** Group Administrators can schedule study sessions within their study groups. Sessions integrate with Google Calendars, and learners receive notifications.
- **Document Sharing:** Users can securely share learning resources within their study groups.

## Nonfunctional Requirements

- **Data Storage:** Application data is stored securely in Cloud Spanner.
- **User Interface:** The UI is intuitive and user-friendly.
- **Security Measures:** Appropriate security measures are implemented to protect user data.
- **Accessibility:** The application is accessible to users with a stable internet connection.
- **Notification System:** Users receive notifications for scheduled study sessions.
- **Scalability:** The application is designed to scale as user activity increases.

## System Architecture

The system follows a modular architecture to ensure scalability and maintainability.

<img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/system-architecture.png"/>

## Data Model

<img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/db-diagram.png"/>

## Solution Structure and Deployment

- **Architecture Overview:** Follows the Model-View-Controller (MVC) architectural pattern.
- **Design Patterns:** Utilizes the Repository pattern for data access.
- **Testing Strategies:** Comprehensive testing strategy includes both unit testing and integration testing.
- **Deployment Approach:** Packaged into a Docker container and deployed using Google Kubernetes Engine (GKE).

## ToDo

- Incorporate testing strategies: Testing needs to be added to ensure reliability and functionality of the application.

## Screenshots

Here are some screenshots of the CoStudy Cloud Application:

1. **Authentication**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/0.Authentication.png"/>

2. **Dashboard**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/1.Dashboard.png"/>

3. **Groups**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/2.Groups.png"/>

4. **Group Creation**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/3.GroupCreation.png"/>

5. **Sessions**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/4.Sessions.png"/>

6. **Session Creation**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/5.SessionCreation.png"/>

7. **Study Session Added to Calendar**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/6.StudySessionAddedToCalendar.png"/>

8. **Documents**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/7.Documents.png"/>

9. **Document Upload**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/8.DocumentUpload.png"/>

10. **Profile**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/9.Profile.png"/>

11. **Users**<br/>
  <img src="https://github.com/ElliotOne/CoStudyCloud/blob/master/docs/screenshots/10.Users.png"/>







