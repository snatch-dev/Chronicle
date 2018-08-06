# Contributing to Chronicle

Do you have great ideas which will make Chronicle even better? That's awesome! Feel free to create **issues or pull request** here on GitHub!

The purpose of this text is to help you start working with the source code without any trouble. Simply, follow the steps below and... have fun :)

## Clone the repository
In order to start working with Chronicle, you need to clone the repository from GitHub using the following command:
```
git clone https://github.com/chronicle-stack/Chronicle.git
```

## Create new branch
After you're done make sure you are one the **develop** branch:
```
git checkout develop
```

Then you can create your own branch for new feature, bug fix or whatever you want. We use very simple naming convention for naming branches:
```
feature/<issue_number_from_github>
```

However, if there's no issue related to your feature you can put the short description instead using **snake case** like in the example below:
```
feature/my_new_awesome_validation_rule
```

Having a proper name for your branch, create it directly from the develop:
```
git checkout -b <name_of_your_branch>
```

## Creating unit tests
We do our best to make Chronicle a reliable library. That's why we pay attention to unit tests for each new functionality. You can find them inside ```tests``` folder. Select a subfolder which should contain new unit tests or create new if none of them suits new functionality. The name of each unit test should follow the convention:
```
Method_Result_When_Condition
```

Here's an example:
```
Float_IsPositive_Fails_When_Given_Value_Is_Null
```

Try to avaoid multiple ```Assert``` inside single unit tests.

When you're done make sure all tests passess. Navigate to the ```Chronicle.Tests``` project and run the following command:
```
dotnet test
```

You can also run the following command for continuous testing:
```
dotnet watch test // this will compile the project and rerun the tests on every file change
```

Alternatively, you can use our **Cake script** which is placed in the root folder. Navigate there and run:
```
./build.sh //on Unix
```
```
./build.ps1 //on Windows
```

## Creating a pull request
When the code is stable, you can submit your changes by creating a pull request. First, push your branch to origin:
```
git push origin <name_of_your_branch>
```

Then go to the **GitHub -> Pull Request -> New Pull Request**.
Select **develop** as base and your branch as compare. We provide default template for PR description:

![PR_Template](http://foreverframe.net/wp-content/uploads/2017/09/Screen-Shot-2017-09-27-at-21.16.02.png)

Make sure:
- PR title is short and concludes work you've done
- GitHub issue number and link is inluded in the description
- You described changes to the codebase

When it's done, simply create your pull request. We use [AppVeyor](https://ci.appveyor.com/project/GooRiOn/chronicle/branch/master)  as CI system and [Codecov](https://codecov.io/gh/chronicle-stack/chronicle/branch/master) as code coverage analyzer. After you push your changes these two tools will take a look at your code. First, the AppVeyor will check whether project builds and all unit tests passess. Then Codecov bot will post a short report which will present code coverage after your changes:

![CC_Report](http://foreverframe.net/wp-content/uploads/2017/10/Screen-Shot-2017-10-22-at-13.13.15.png)

Each PR must fulfill certain conditions before it can be merged:

- The build must succeed on AppVeyor
- Code coverage can't be discreassed by the PR
- One of the owners must approve your changes


If some of the above won't be fulfilled (due to change request or some mistake) simply fix it locally on your machine, create new commit and push it to origin. This will update the exisitng PR and will kick off all checks again.

If everything will be fine, your changes will be merged into develop, branch will be deleted and related issue will be closed.

# WELL DONE AND THANK YOU VERY MUCH!





