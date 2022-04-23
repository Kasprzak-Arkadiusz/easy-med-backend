heroku login
heroku container:login
heroku container:push web -a easy-med-api
heroku container:release web -a easy-med-api