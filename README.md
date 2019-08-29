# Sudoku API

Basic web API which validate and generate Sudoku grids.

## Requisites

.NET Core 3 (preview or later) is required in order to build & run this project.

## Run & Deploy

*   Clone the repository
*   Run command `dotnet restore`
*   Then run `dotnet run`

## How to validate a grid

*   Send a grid as a POST request to "api/sudoku/isValid" like this :

        {
            "values": [ [ 1, 2, 3, ..., 8, 9], ..., [ 3, 4, 5, ..., 8, 4 ] ]
        }

    The server will send back :

    1. an HTTP 200 if the grid is valid

    OR

    2. an HTTP 400 with an error message if the grid is not valid

## How to generate a new grid

* 	Send a GET request to "api/sudoku".

	The server will send back an HTTP 200 with the generated value as payload.

## How it works

*	The grid validation algorithm will extract all the lines, rows and "square regions" and will compare all of them with the allowed values. If all this lists are correct then the grid is valid.
	The extraction process and the validation are done exclusively in LINQ. No for or while loops at all.

*	The grid generation algorithm follows the backtracking principle. It tries one by one every tile with their possible values.
	If at one moment, we tried every possibilities for a tile, then we "move backward" in found values and we try with another possible value, and we continue like that until the grid is fulfilled.
	With this principle, we can generate a new grid pretty fast (around 30-40 ms).

## Few CURLs for testing purpose

- Successful
```shell
curl -X POST -H "Content-Type: application/json" -H "Cache-Control: no-cache" -d '{
    "values": [
        [8, 6, 4, 9, 3, 2, 7, 1, 5],
        [1, 3, 5, 7, 8, 6, 4, 9, 2],
        [9, 2, 7, 1, 5, 4, 8, 3, 6],
        [3, 4, 6, 2, 7, 8, 1, 5, 9],
        [7, 5, 1, 4, 9, 3, 6, 2, 8],
        [2, 9, 8, 6, 1, 5, 3, 4, 7],
        [5, 7, 2, 8, 4, 1, 9, 6, 3],
        [6, 1, 9, 3, 2, 7, 5, 8, 4],
        [4, 8, 3, 5, 6, 9, 2, 7, 1]
    ]
}' "http://localhost:5000/api/sudoku/isValid"
```
```shell
curl -X GET -H "Cache-Control: no-cache" "http://localhost:5000/api/sudoku"
```
- Error
```shell
curl -X POST -H "Content-Type: application/json" -H "Cache-Control: no-cache" -d '{
    "values": [
        [1, 2, 3],
        [4, 5, 6],
        [7, 8, 9]
    ]
}' "http://localhost:5000/api/sudoku/isValid"
```
```shell
curl -X POST -H "Content-Type: application/json" -H "Cache-Control: no-cache" -d '{
    "values": [
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0]
    ]
}' "http://localhost:5000/api/sudoku/isValid"
```