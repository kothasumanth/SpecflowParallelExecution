Feature: Launch webpage and validate some elements
This featuer is to launch browser and validate some elements on the webpage

@Launch
Scenario: Launch the browser and validate the elements
Given Login to the webpage
When i navigate to some page
Then element should have this