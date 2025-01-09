
# Virtual Zoo Project




## Introduction

This project is the second part of our final assignment in the C# learning trajectory. The goal is to apply our C# knowledge and skills by developing a web application and an API. In this assignment, we demonstrate our proficiency in the basic aspects of C# programming as well as the frameworks and tools associated with it.

Api Docs
[Link to Documentation](#api-docs)

## Project Description

We have created a virtual zoo where users can manage the zoo. It is possible to create animals, set up enclosures for the animals, and categorize the animals for easy searching. Additionally, users can perform various actions such as:

- Creating a new layout
- Completing an existing layout
- Setting feeding times
- Triggering sunrise and sunset
- Checking if all conditions are met

## Frameworks and Tools Used
- ASP.NET Core Web App (Model-View-Controller): We implemented the MVC design pattern to structure our application.
- Entity Framework: Used for database management, allowing us to perform CRUD operations efficiently.
- .NET 8.0: The latest version of .NET, providing improved performance and new features.
- API Controllers: Built a robust API to handle all user actions and operations.
- Razor Pages: Used for creating dynamic web pages.
- Migrations and Seeding-Data: Implemented to ensure the database can be set up and populated with initial data easily.

## Key Functionalities
*Animals (Animal)*

CRUD operations on animals
Searching/filtering by properties
Actions such as Sunrise, Sunset, Feeding time, and CheckConstraints

*Categories (Category)*

CRUD operations on categories
Assigning animals to a category
Searching/filtering by category

*Enclosures (Enclosure)*

CRUD operations on enclosures
Assigning animals to an enclosure
Actions such as Sunrise, Sunset, Feeding time, and CheckConstraints

*Zoo (Zoo)*

Actions such as Sunrise, Sunset, Feeding time, and CheckConstraints
AutoAssign action for automatically assigning animals to enclosures


## What We Have Learned

During this project, we learned how to develop a web application and API using C#. We gained experience in applying the MVC design pattern, using Entity Framework for database management, and implementing various functionalities such as CRUD operations and user actions. Additionally, we learned how to structure our code according to Microsoft coding conventions and how to build a secure and efficient web application.
## 🛠 Skills
C#, .Net, API, Entity Framework, SQL Server, MVC design, Bogus

# API Docs


### Animals

#### Get all animals

  GET /api/animals

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
|  |  |  |

#### Get animal

  GET /api/animals/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of animal to fetch |

#### Search animals

  GET /api/animals/search?

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `name`    | `string` | **Required**. Name of the animal  |
| `species` | `string` | **Required**. Species of the animal |
| `category`| `string` | **Required**. Category of the animal |
| `size`    | `string` | **Required**. Size of the animal  |
| `diet`    | `string` | **Required**. Diet of the animal  |
| `activity`| `string` | **Required**. Activity pattern of the animal |
| `prey`    | `string` | **Required**. Prey of the animal  |
| `enclosure`| `string`| **Required**. Enclosure of the animal |
| `security`| `string` | **Required**. Security requirement of the animal |

#### Create animal

  POST /api/animals

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `name`    | `string` | **Required**. Name of the animal  |
| `species` | `string` | **Required**. Species of the animal |
| `categoryId`| `int`  | **Required**. Category ID of the animal |
| `animalSize`| `int`  | **Required**. Size of the animal  |
| `animalDiet`| `int`  | **Required**. Diet of the animal  |
| `animalActivityPattern`| `int` | **Required**. Activity pattern of the animal |
| `prey`    | `string` | **Required**. Prey of the animal  |
| `enclosureId`| `int` | **Required**. Enclosure ID of the animal |
| `spaceRequirement`| `double` | **Required**. Space requirement of the animal |
| `securityRequirement`| `int` | **Required**. Security requirement of the animal |

#### Update animal

  PUT /api/animals/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of animal to update |
| `name`    | `string` | **Required**. Name of the animal  |
| `species` | `string` | **Required**. Species of the animal |
| `categoryId`| `int`  | **Required**. Category ID of the animal |
| `animalSize`| `int`  | **Required**. Size of the animal  |
| `animalDiet`| `int`  | **Required**. Diet of the animal  |
| `animalActivityPattern`| `int` | **Required**. Activity pattern of the animal |
| `prey`    | `string` | **Required**. Prey of the animal  |
| `enclosureId`| `int` | **Required**. Enclosure ID of the animal |
| `spaceRequirement`| `double` | **Required**. Space requirement of the animal |
| `securityRequirement`| `int` | **Required**. Security requirement of the animal |

#### Delete animal

  DELETE /api/animals/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of animal to delete |

### Categories

#### Get all categories

  GET /api/categories

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
|  |  |  |

#### Get category

  GET /api/categories/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of category to fetch |

#### Search categories

  GET /api/categories/search?

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `name`    | `string` | **Required**. Name of the category |

#### Create category

  POST /api/categories

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `name`    | `string` | **Required**. Name of the category |
| `animals` | `array`  |  Array of animal objects |

#### Update category

  PUT /api/categories/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of category to update |
| `name`    | `string` | **Required**. Name of the category |
| `animals` | `array`  |  Array of animal objects |

#### Delete category

  DELETE /api/categories/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of category to delete |

### Enclosures

#### Get all enclosures

  GET /api/enclosures

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
|  |  |  |

#### Get enclosure

  GET /api/enclosures/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of enclosure to fetch |

#### Search enclosures

  GET /api/enclosures/search

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `name`    | `string` | **Required**. Name of the enclosure |
| `zoo`     | `string` | **Required**. Zoo of the enclosure |
| `climate` | `string` | **Required**. Climate of the enclosure |
| `habitat` | `string` | **Required**. Habitat of the enclosure |
| `security`| `string` | **Required**. Security level of the enclosure |
| `space`   | `double` | **Required**. Space requirement of the enclosure |

#### Create enclosure

  POST /api/enclosures

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `name`    | `string` | **Required**. Name of the enclosure |
| `zooId`   | `int`    | Zoo ID of the enclosure |
| `animals` | `array`  | **Required**. Array of animal objects |
| `enclosureClimate`| `int` | **Required**. Climate of the enclosure |
| `enclosureHabitatType`| `int` | **Required**. Habitat type of the enclosure |
| `enclosureSecurityLevel`| `int` | **Required**. Security level of the enclosure |
| `size`    | `double` | **Required**. Size of the enclosure |

#### Update enclosure

  PUT /api/enclosures/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of enclosure to update |
| `name`    | `string` | **Required**. Name of the enclosure |
| `zooId`   | `int`    | Zoo ID of the enclosure |
| `animals` | `array`  | **Required**. Array of animal objects |
| `enclosureClimate`| `int` | **Required**. Climate of the enclosure |
| `enclosureHabitatType`| `int` | **Required**. Habitat type of the enclosure |
| `enclosureSecurityLevel`| `int` | **Required**. Security level of the enclosure |
| `size`    | `double` | **Required**. Size of the enclosure |

#### Delete enclosure

  DELETE /api/enclosures/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of enclosure to delete |

### Zoos

#### Get all zoos

  GET /api/zoos

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
|  |  |  |

#### Get zoo

  GET /api/zoos/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of zoo to fetch  |

#### Search zoos

  GET /api/zoos/search?

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `name`    | `string` | **Required**. Name of the zoo     |

#### Create zoo

  POST /api/zoos

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `name`    | `string` | **Required**. Name of the zoo     |
| `enclosures` | `array` | Array of enclosure objects |

#### Update zoo

  PUT /api/zoos/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of zoo to update |
| `name`    | `string` | **Required**. Name of the zoo     |
| `enclosures` | `array` |  Array of enclosure objects |

#### Delete zoo

  DELETE /api/zoos/{id}

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Required**. Id of zoo to delete |


