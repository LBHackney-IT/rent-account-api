.PHONY: setup
setup:
	docker-compose build

.PHONY: build
build:
	docker-compose build rent_account_api

.PHONY: serve
serve:
	docker-compose build rent_account_api && docker-compose up rent_account_api

.PHONY: shell
shell:
	docker-compose run rent_account_api bash

.PHONY: test
test:
	docker-compose up test-database & docker-compose build rent_account_api-test && docker-compose up rent_account_api-test

.PHONY: lint
lint:
	-dotnet tool install -g dotnet-format
	dotnet tool update -g dotnet-format
	dotnet format
