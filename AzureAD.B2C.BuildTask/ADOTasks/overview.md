# Azure AD B2C
There are two task available.
1. #### Build AD(B2C) Policies
    it will replace all the environment related configuration and save policies in artifect directory.
    
2. #### Release AD(B2C) Policies
    it will create/update policies and [encryption keys](https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-get-started?tabs=applications#add-signing-and-encryption-keys) in azure


To know more about azure b2c custom policies, please click [here](https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-get-started-custom?tabs=applications).

#### *Sample YAML file is available on github.* 
## Please Visit [Microdoft Docs](https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-get-started?tabs=applications#register-identity-experience-framework-applications) for B2C Setup
# Installation and Configuration
#### 1. Register An Application And Give Graph permissions
1. Go to azure b2c tenant
2. Register an application in b2c tenant
    - Go to Azure Active Directory in b2c
    - Click on **App registrations (Legacy)** **1*
    ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/appreg1.png)
    - Click on **New application registration** **2*
    - Fill the details 
        - Name - Name of the application **1*
        - Application type - type of the application, select Web app/ API **2*
        - Sign-On URL - URL of you application (you can give any url) **3*
     - Click on **Create** button **4*
    ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/appreg2.png)
3. Now Click on **Settings**
    - copy the Application Id for future use
![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/appreg3.png)
4. Permissions
    - Click on **Required permissions** **1*
        - we are using Microsoft Graph API's for creation and updation so for that we need some permissions. 
    - Click on **Add** **2*
    ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/appreg4.png) 
    - Click on **Select an API** and Select **Microsoft Graph**
      ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/appreg5.png) 
    - After selecting Microsoft Graph it will ask you for Permissions
        - Give Following Permissions
            - Delegate 
                - Read and write your organization's trust framework policies 
                - Read your organization's policies
                - Read trust framework key sets
                - Read and write trust framework key sets
            - Application
                - Read and write your organization's trust framework policies 
                - Read your organization's policies
                - Read trust framework key sets
                - Read and write trust framework key sets

        **Make Sure that Microsoft graph is showing Total 8 Permissions**
        ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/appreg8.png)

    - Click on **Done**  **1*
    - Click on **Grant permissions**  **2*
        - *this is most important step, please make sure this is done*
    ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/appreg6.png)
    - Now we will generate secret/Key
        - Select **Keys** **1*
        - Fill Details **2*
            - Key description 
            - Duration - please select Never expire
        - save and copy the secret for future use
    ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/appreg7.png)
   
   #### 2. Build Task
   1. Create Build Pipeline
   2. Search Build AD(B2C) Policies
   3. Insert Details
        - Policy Directory Path - Path for your policies folder in Repo
        - JSON Values - Json Values<br> 
            {<br>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"Tenant":"B2CTEST",<br>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            "FacebookCliendId":"12121",<br>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            "FacebookSecret":"asa1231"<br>
            } 
        - Artifact Publish Path - Artifect publish path (used by next)
    4. Add new task (Publish Artifects)
    5. save and run

   ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/build1.png)

    #### 2. Release Task
   1. Create Release Pipeline
   2. Add Artifects (Save artifects path)
   3. Add Task (Search Release AD(B2C) Policies)
   3. Insert Details
        - B2C Domain/ Tenant name - B2c Domain name
        - Application Id - Application Id 
        - Application Secret - Application Secret
        - Artifact Publish Path - Path from above task
    5. save and run

    ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/releasepipeline.png)<br>
    ![image](https://raw.githubusercontent.com/manishtiwari25/AzureAD.B2C.BuildTask/master/AzureAD.B2C.BuildTask/ADOTasks/images/release2.png)
