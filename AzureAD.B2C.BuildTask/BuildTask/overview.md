# Azure AD(B2C) Custom Policy Task
custom policy task is a utility to deploy b2c custom policies.
this build task enable following features
1. it will replace all the environment related configuration
2. it will create/update policies in azure 

# Installation and Configuration
1. Go to azure b2c tenant
2. Register an application in b2c tenant
    - Go to Azure Active Directory in b2c
    - Click on **App registrations (Legacy)** **1*
    ![image](images/appreg1.png)
    - Click on **New application registration** **2*
    - Fill the details 
        - Name - Name of the application **1*
        - Application type - type of the application, select Web app/ API **2*
        - Sign-On URL - URL of you application (you can give any url) **3*
     - Click on **Create** button **4*
    ![image](images/appreg2.png)
3. Now Click on **Settings**
    - copy the Application Id for future use
![image](images/appreg3.png)
4. Permissions
    - Click on **Required permissions** **1*
        - we are using Microsoft Graph API's for creation and updation so for that we need some permissions. 
    - Click on **Add** **2*
    ![image](images/appreg4.png) 
    - Click on **Select an API** and Select **Microsoft Graph**
      ![image](images/appreg5.png) 
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
        ![image](images/appreg8.png)

    - Click on **Done**  **1*
    - Click on **Grant permissions**  **2*
        - *this is most important step, please make sure this is done*
    ![image](images/appreg6.png)
    - Now we will generate secret/Key
        - Select **Keys** **1*
        - Fill Details **2*
            - Key description 
            - Duration - please select Never expire
        - save and copy the secret for future use
    ![image](images/appreg7.png)

    ![image](images/extension1.png)
   