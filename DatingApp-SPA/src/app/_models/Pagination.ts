export interface Pagination {  // changed from interface
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;

}

export class PaginatedResult<T> {
    result: T;
    pagination: Pagination;
  messages: any;
}

