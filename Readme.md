# Запуск 

Просто соберите образ:

```bash
$ docker build . -t setyourtone
```
И запустите контейнер:
```bash
$ docker run --rm -it -p 8080:80 setyourtone
```

и подключитесь к `localhost:8080`
