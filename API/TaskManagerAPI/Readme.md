# **[IN PROGRESS]** Using AuthorizeFilter layer in .Net Core Authentication

> This is a simple  example of how to add an extra layer to the default net core JWT token authentication for two specific cases: Check the token in the DB and check if the user is active. **THIS README IS IN PROGRESS OF BEEN FINISHED.**

---

## Table of Contents

- [Description](#description)
- [Code](#code)
- [How To Use](#how-to-use)
- [References](#references)
- [Author Info](#author-info)

---

## **Description**

### **Prerequisites**

- Knowledge of what is a json web token (JWT). If not, Just keep in mind two things: it is used to validate the user one each request and also that it  expires.

- Knowledge of the execution of how filters work in ASP MVC.

I have a attached a link with explanations  in the [references](#references) section.

### **Scenarios**

In a lot of applications jwt tokens  are used to authenticate a logged user on each request. The next is an example of how the authentication is done in those cases:

1. [FE] User goes to the login page, fill the form with  its user and password. Those are send by FE (front-end, client side) to the BE (back-end, server side)
2. [BE] Authenticate the user and returns a jwt token that FE will store
3. [FE] For each request send to BE the token is attached to authenticate the user

But what if the user logs out? In a log out request BE trust FE would delete the token, this means **FE is responsible of the sing out**. Also, if a user is logged in and an administrator has disable that user by any reason, the token will continue be valid until it expires.

This project aims to avoid the previous two cases by adding an extra layer defined in the **_AuthenticationFilter_** class that extends **_AuthorizeFilter_**  class of Microsoft.AspNetCore.Mvc.Authorization.  

- The first case will be solved storing the token in a db (database) when the user has logged in and, on each request, verifying it.
- The second case will be solved checking on each request that the user is enabled (active)

Both solutions will decrease the performance  however, it add more security.

## **Code**

### **Technologies**

- ASP NET Core 2.2
- JWT token

[Back To The Top](#read-me-template)

---

## **How To Use**

### **Installation**

Just download and execute it. The DB used is created by Entity Framework in memory. In case you want to use your own DB you should change the next properties in the _appsettings.json_:

1. Set the property "UseInMemoryDB" to false

2. Set your database connection string  in the variable "connectionString"

### **Main files**

#### **AuthenticationFilter**

### **Other files**

#### **JwtAuthenticationExtension**

```html
    <p>dummy code</p>
```

[Back To The Top](#read-me-template)

---

## References

- [5 Easy Steps to Understanding JSON Web Tokens (JWT)](https://medium.com/vandium-software/5-easy-steps-to-understanding-json-web-tokens-jwt-1164c0adfcec)

- [JWT RFC](https://tools.ietf.org/html/rfc7519)

- [How to log out when using JWT](https://medium.com/devgorilla/how-to-log-out-when-using-jwt-a8c7823e8a6)

- [Authorization Filter](https://docs.microsoft.com/es-es/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.2#authorization-filters)

- [How filters work](https://docs.microsoft.com/es-es/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.2#how-filters-work)

[Back To The Top](#read-me-template)

---

## Author Info

- Twitter - [@AnguloMascarell](https://twitter.com/angulomascarell)
- Linkedin - [Carlos Angulo](https://www.linkedin.com/in/angulomascarell)

[Back To The Top](#read-me-template)
