# Dru's Delirium and Other Coding Antics

This project is an attempt to solve two birds with one stone; finally getting a website up and running
whilst simultaneously exploring ASP.NET Core 3.1.

## Goals

1. To create a modern fully-async blog with a minimal feature set.
2. To explore .NET Core and ASP.NET Core.
3. To integrate wtih various tools to provide continuous integration and deployment.
4. To have fun doing so.

## Setup

The application runs in a docker container on http://andrewjsaid.com.

Currently it is accessible behind an Nginx reverse-proxy as recommended by MS for the initial ASP.NET Core release.
The nginx server handles certificates and SSL-related functionality.

The blog content is stored in a git repository https://github.com/andrewjsaid/drudoca.blog-posts.

## Deployments

Run the following instructions on the server to deploy a new version.

    docker pull andrewjsaid/drudoca
    docker stop drudoca
    docker rm drudoca
    docker run --name drudoca -p 5001:80 -d -v /home/svc/drudoca.blog-posts/posts:/app/wwwroot/blog-posts -v /home/svc/drudoca.blog-posts/images:/app/wwwroot/blog-images --restart always andrewjsaid/drudoca

Run the following instructions on the server to update the blog content.

    cd /home/svc/drudoca.blog-posts
    git pull
