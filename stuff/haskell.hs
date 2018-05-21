{- Haskell comment
-}

-- single comment

import Data.List
import System.IO

maxInt = maxBound :: Int
minInt = minBound :: Int

boolValue = True && False

constantValue :: Int
constantValue = 5

mySum = sum [1, 10]

divNum = 5 / 4

num9 :: Int
num9 = 9
sqrtOf9 = sqrt(fromIntegral num9)

-- truncate, round, ceiling, floor, exp, log, sin, not(boolean expression)

list = [1, 24, 54, 453]
list2 = list ++ [124, 42]

-- : [] end if list
aList = 12 : 3 : 5 : []
listOfList = [[12, 4], [1, 2 , 3]]

-- length, reverse, null, 

-- access element
secondElem = listOfList !! 1

-- head, last, init, take, drop, `elem` to check if an element is present

-- sum, product

letterList = ['A', 'F' .. 'Z']

infiniteList = [10, 20 ..]

-- take, repeat, cycle, 

anotherList = [ x * 3 | x <- [1..10], x * 3 <= 15]

-- sort

combined = zipWith (*) [1, 2, 3] [3, 4, 3, 6]
filtered = filter ( > 3) [1, 2, 3, 4, 5]
another = takeWhile( <= 20) [2, 4..]

multiple = foldl (*) 10 [2, 3, 4, 5]

fibonacci = 1 : 1 : zipWith (+) fibonacci (tail fibonacci)

powerOf3 = [3^n | n <- [1..10]]

multTable = [[x * y | y <- [1..10]] | x <- [1..10]]

randTuple = (1, "string")

id = fst randTuple
string = snd randTuple

names = [1, 2, 3, 4]
namesTuple = zip names names

-- functions

main = do
    putStrLn "Hello"
    name <- getLine
    putStrLn("Hello " ++ name)

addMe :: Int -> Int -> Int
-- funcNmae param1 param3 = operatsions (returned value)
addMe x y = x + y

-- general sum
sumMe x y = x + y

factorial :: Integer -> Integer
factorial 0 = 1
factorial n = n * factorial (n-1)

findZero :: Integer -> Integer
findZero x = if mod x 10 == 0 then 1 + findZero (div x 10) else 0

isOdd :: Int -> Bool
isOdd n 
    | remainder == 0 = False 
    | otherwise = True
    where remainder = mod n 2

doubleTuple = (1, "Had")
tripleTuple = (1, "had", 4.3)

getItems :: [Int] -> String
getItems [] = "Empty"
getItems (x:other) = "First is " ++ show x

addTre :: Int -> Int
addTre x = x + 3
ones = [1, 1, 1, 1]
three = map addTre ones

otherTree :: [Int] -> [Int]
otherTree [] = []
otherTree (x:other) = addTre x : otherTree other


anotherFunc = map (\x -> x * 2) [1..10]

switch n = case n of
    5 -> "Case is five"
    _ -> "anything"

-- enumeration
data Player = Football
            | Runner
            deriving Show

aFunction :: Player -> Bool
aFunction Runner = True
aFunction _ = False

data Customer = Customer String String Double
    deriving Show

dummy = Customer "user" "player" 5.0
getBalance :: Customer -> Double
getBalance (Customer _ _ x) = x

data Shape = Circle Float Float Float | Rectangle Float Float Float Float
    deriving Show

area :: Shape -> Float
area (Circle _ _ r) = pi * r / 2
area (Rectangle x1 y1 x2 y2) = (abs $ x1 - y1) * (abs $ x2 - y2)

data Struct = Struct {
    name :: String,
    idz :: Int
} deriving (Eq, Show)

aStruct = Struct { name = "Todd", idz = 1 }

data ShirtSize = S | M | L
instance Eq ShirtSize where
    S == S = True
    M == M = True
    L == L = True
    _ == _ = False

instance Show ShirtSize where
    show S = "Small"
    show M = "Medium"
    show L = "Large"

class MyEq a where
    areEqual :: a -> a -> Bool

instance MyEq ShirtSize where
    areEqual S S = True

-- writing on file
writeToFile = do
    theFile <- openFile "text.txt" WriteMode
    hPutStrLn theFile ("Random")
    hClose
    -- hGetContents

    