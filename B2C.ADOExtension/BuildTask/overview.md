# buildThing: Dynamics 365

_buildThing: Dynamics 365_ is a set of tasks which aim to allow a full ‘hands-off’ deployment of Dynamics CRM/365 for Customer Engagement release by cloning a known good master instance to a target instance.

## Show me the Things!

|Thing Is|Thing Does|
|---|---|
| Drift the Data | Copies config data from a specified entity, filtered by a view |
|User Switcheroo|Looks for every plugin step/workflow in the source instance set to run as a specified user and switches the user to be one in the target instance.|
|Fix a Field|Updates the value of a single specific field in a specific single record|
|Prevent Plagiarism|Grabs all **published** duplicate detection rules and the associated conditions and creates them in the target with the same GUIDs.|
|Let Me In|Looks for all entities with an Access Team enabled, copies over the templates and creates them with the original GUID|
|Wake Up Your Logic|Activates all Business Rules in the target|
|Facsimile Your Stencils|Copies all Word Document Templates, updating the Entity Type Code inside the template if necessary.  With thanks to Gayan Perera [@NZxRMGuy](https://twitter.com/@NZxRMGuy) and magnetismsolutions.com for permission to use his code for this|


## Using the Things
You can use buildThing things in a build or release job in VSTS.  Here's an example of a release pipeline, in which we've set up a chain of D365CE instances, in each of which we want to run a selection of buildThing things:

![Release Pipeline](Images/pipeline.png)

If we drill down into the 'staging' environment, here's a set of tasks we might want to run there:

![Release tasks](Images/tasklist.png)

The last task in the list shows a typical config screen for a buildThing thing - in this case we have a configuration entity which stores a Google Maps API Key, and we want to substitute in the right one for this particular D365CE instance.

## Version History

<dl>
<dt>v0.2 (Preview) Minor update</dt>
<dd>No functional changes - just re-compiled to make compatible with v9.  They still work for v8.2 as well</dd>
<dl>
<dt>v0.0.1 (Preview) Initial Release</dt
><dd>We're using these tools every day in VSTS for v8.2 instances.  They should work in TFS and with v8.1 of D365CE, but in all cases should only be considered as suitable for evaluation and feedback.</dd>

### Open Source ###
We'll be releasing the code for these tasks on GitHub in due course.

## About cloudThing
cloudThing is a Software Development, Dynamics 365 and DevOps company headquartered in the UK with offices in India. We solve complex business problems with simple to use technology. Not everything we produce is free, but this is, you can visit [www.cloudthing.com](https://www.cloudthing.com) to find out more. 


