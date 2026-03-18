import { BasketItem } from "src/app/core/models/basket.model";
import { Book } from "src/app/core/models/book.model";

export function isBook(item: Book | BasketItem): item is Book {
    return (item as Book).id !== undefined
}