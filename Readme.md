# Запуск 

Просто соберите образ:

```bash
$ docker build . -t setyourtone
```
И запустите контейнер:
```bash
$ docker run --rm -it -p 80:8080 setyourtone
```

и подключитесь к `localhost:8080`
